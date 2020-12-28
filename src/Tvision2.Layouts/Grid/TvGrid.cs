using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Xml.Serialization;
using Tvision2.Core.Colors;
using Tvision2.Core.Components;
using Tvision2.Core.Engine;
using Tvision2.Core.Render;
using Tvision2.Styles.Extensions;

namespace Tvision2.Layouts.Grid
{

    public interface IGridContainer
    {
        public void Add(TvComponent child);
        public IGridContainer WithAlignment(ChildAlignment alignment);
    }

    public class TvGrid : ITvContainer, IGridContainer
    {
        private readonly TvComponent<GridState> _component;
        private readonly TvGridComponentTree _childs;
        private readonly GridState _state;
        public static TvGridBuilder With() => new TvGridBuilder();
        public string Name { get; }
        private bool _isMounted;
        private readonly List<TvComponent> _pendingAdds;
        private ComponentTree _tree;
        public TvComponent AsComponent() => _component;
        private readonly TvGridOptions _options;

        private TvBounds[,] _cellBounds;


        private int _currentRow;
        private int _currentCol;
        private ChildAlignment _currentAlignment;


        public TvGrid(GridState state, TvGridOptions options, string name)
        {
            _options = options ?? new TvGridOptions();
            _currentAlignment = _options.DefaultAlignment;
            _pendingAdds = new List<TvComponent>();
            _isMounted = false;
            _state = state;
            _component = new TvComponent<GridState>(_state, name,
                cfg =>
                {
                    cfg.WhenChildMounted(ctx => ResizeRowsAndCols());
                    cfg.WhenChildUnmounted(ctx => ResizeRowsAndCols());
                    cfg.WhenComponentMounted(ctx => OnComponentMounted(ctx));
                });
            Name = _component.Name;
            if (_options.Border.HasVerticalBorder || _options.Border.HasHorizontalBorder)
            {
                _component.AddDrawer(ctx => OnDrawBorder(ctx));
            }

            _component.Metadata.ViewportChanged += OnViewportChanged;
            _childs = new TvGridComponentTree();
            _cellBounds = new TvBounds[state.Rows, state.Cols];
            RecalculateBounds();
        }

        private void RecalculateBounds()
        {
            var vport = AsComponent().Viewport;
            if (vport == null) return;
            var bounds = vport.Bounds;

            var totalWidthForCells = _options.Border.HasHorizontalBorder ? bounds.Cols - (_state.Cols + 1) : bounds.Cols;
            var totalHeightForCells = _options.Border.HasVerticalBorder ? bounds.Rows - (_state.Rows + 1) : bounds.Rows;

            var cellWidth = totalWidthForCells / _state.Cols;
            var cellHeight = totalHeightForCells / _state.Rows;

            var remHeight = totalHeightForCells;
            for (var row = 0; row < _state.Rows; row++)
            {

                var thisRowHeight = row == _state.Rows - 1 ? remHeight : cellHeight;
                var remWidth = totalWidthForCells;
                for (var col = 0; col < _state.Cols; col++)
                {
                    var thisColWidth =col == _state.Cols -1  ? remWidth : cellWidth;
                    _cellBounds[row, col] = TvBounds.FromRowsAndCols(thisRowHeight, thisColWidth);
                    remWidth -= cellWidth;
                }
                remHeight -= cellHeight;
            }
        }

        private void OnDrawBorder(RenderContext<GridState> ctx)
        {
            var (borderHor, borderVer) = _options.Border;
            var attr = _options.BorderAttribute;
            for (var row = 0; row < ctx.Viewport.Bounds.Rows; row++)
            {
                if (RowHasHorizontalBorder(row))
                {
                    ctx.DrawChars('=', ctx.Viewport.Bounds.Cols, TvPoint.FromXY(0, row), attr);
                }
                for (var col = 0; col < ctx.Viewport.Bounds.Cols; col ++)
                {
                    if (ColHasVerticalBorder(col))
                    {
                        ctx.DrawChars('|', 1, TvPoint.FromXY(col, row), attr);
                    }
                }
            }


            bool RowHasHorizontalBorder(int row)
            {
                if (!_options.Border.HasHorizontalBorder) return false;

                if (row == 0 || row == ctx.Viewport.Bounds.Rows - 1) return true;
                var accrow = 0;
                for (var hrow =0; hrow <_state.Rows; hrow++)
                {
                    accrow += _cellBounds[hrow, 0].Rows + 1;
                    if (row == accrow) return true;
                }
                return false;
            }

            bool ColHasVerticalBorder(int col)
            {
                if (!_options.Border.HasVerticalBorder) return false;
                if (col == 0 || col == ctx.Viewport.Bounds.Cols - 1) return true;
                var acccol = 0;
                for (var hcol = 0; hcol < _state.Cols; hcol++)
                {
                    acccol += _cellBounds[0, hcol].Cols + 1;
                    if (col == acccol) return true;
                }
                return false;

            }

        }

        private void OnComponentMounted(ComponentMoutingContext ctx)
        {
            _tree = ctx.ComponentTree;
            _isMounted = true;

            if (_pendingAdds.Any())
            {
                foreach (var cmpToAdd in _pendingAdds)
                {
                    DoPendingAdd(cmpToAdd);
                }
                _pendingAdds.Clear();
                ResizeRowsAndCols();
            }
        }

        private void OnViewportChanged(object sender, ViewportUpdatedEventArgs e)
        {
            RecalculateBounds();
            ResizeRowsAndCols();
        }


        public IGridContainer At(int row, int col)
        {
            _currentRow = row;
            _currentCol = col;
            return this;
        }


        IGridContainer IGridContainer.WithAlignment(ChildAlignment alignment)
        {
            _currentAlignment = alignment;
            return this;
        }

        private void ResizeRowsAndCols()
        {
            var myViewport = AsComponent().Viewport;
            if (myViewport == null)
            {
                return;
            }

            var cellHeight = myViewport.Bounds.Rows / _state.Rows;
            var cellWidth = myViewport.Bounds.Cols / _state.Cols;

            foreach (var kvpChild in _childs.Values)
            {
                var ctrRow = kvpChild.Key.Row;
                var ctrCol = kvpChild.Key.Column;
                var childComponent = kvpChild.Value.Component;
                if (childComponent.Viewport != null)
                {
                    var viewport = CalculateViewportFor(myViewport, ctrRow, ctrCol, cellHeight, cellWidth, kvpChild.Value);
                    childComponent.UpdateViewport(viewport, addIfNotExists: false);
                }
            }
        }

        private IViewport CalculateViewportFor(IViewport myViewport, int ctrRow, int ctrCol, int cellHeight, int cellWidth, TvGridComponentTreeEntry entry)
        {
            var childViewport = entry.Component.Viewport;

            var alignment = entry.Alignment;
            var innerViewport = childViewport ?? Viewport.NullViewport;

            TvPoint cellPos = TvPoint.FromXY(ctrCol * cellWidth, ctrRow * cellHeight);
            TvBounds cellBounds = _cellBounds[ctrRow, ctrCol];
            var horDespl = _options.Border.HasHorizontalBorder ? 1 : 0;
            var verDespl = _options.Border.HasVerticalBorder ? 1 : 0;
            TvPoint childDisplacement = entry.ViewportOriginal ? childViewport.Position : TvPoint.FromXY(horDespl, verDespl);

            var viewport = alignment switch
            {
                ChildAlignment.None => myViewport.InnerViewport(cellPos + childDisplacement, innerViewport.Bounds),
                ChildAlignment.StretchHorizontal => myViewport.InnerViewport(cellPos + TvPoint.FromXY(0, childDisplacement.Top), TvBounds.FromRowsAndCols(cellBounds.Rows, innerViewport.Bounds.Cols)),
                ChildAlignment.StretchVertical => myViewport.InnerViewport(cellPos + TvPoint.FromXY(childDisplacement.Left, 0), TvBounds.FromRowsAndCols(innerViewport.Bounds.Rows, cellBounds.Cols)),
                _ => new Viewport(cellPos, cellBounds, childViewport.ZIndex, childViewport.Flow)
            };

            entry.ViewportOriginal = false;

            return viewport;
        }


        private void DoPendingAdd(TvComponent cmpToAdd)
        {
            _tree.AddAsChild(cmpToAdd, AsComponent());
        }

        void IGridContainer.Add(TvComponent child)
        {
            _childs.Set(_currentCol, _currentRow, child, _currentAlignment);
            if (_isMounted)
            {
                _tree.AddAsChild(child, AsComponent());
            }
            else
            {
                _pendingAdds.Add(child);
            }

            _currentAlignment = _options.DefaultAlignment;
        }

        public bool Remove(TvComponent component) => _childs.Remove(component);

        public void Clear() => _childs.Clear();

        public int Count { get => _childs.Count; }

    }
}
