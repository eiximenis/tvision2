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
using Tvision2.Styles;
using Tvision2.Styles.Extensions;

namespace Tvision2.Layouts.Grid
{

    public interface IRowColSpecifier<T> where T : class
    {
        IRowSpecifier<T> Col(int index);
        IColSpecifier<T> Row(int index);
    }

    public interface IColSpecifier<out T> where T : class
    {
        T Col(int index);
    }
    public interface IRowSpecifier<out T> where T: class
    {
        T Row(int index);
    }

    public interface IGridContainer
    {
        public void Add(TvComponent child);
        public IGridContainer WithAlignment(ChildAlignment alignment);
    }

    public class TvGrid : ITvContainer, IGridContainer, IRowColSpecifier<IGridContainer>, IColSpecifier<IGridContainer>, IRowSpecifier<IGridContainer>
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
        private readonly char[] _borderSet;


        private int _currentRow;
        private int _currentCol;
        private ChildAlignment _currentAlignment;


        public TvGrid(GridState state, TvGridOptions? options, string name)
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
            if (_options.Border.HasBorder)
            {
                _borderSet = BorderSets.GetBorderSet(_options.Border);
            }
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
                    var thisColWidth = col == _state.Cols - 1 ? remWidth : cellWidth;
                    _cellBounds[row, col] = TvBounds.FromRowsAndCols(thisRowHeight, thisColWidth);
                    remWidth -= cellWidth;
                }
                remHeight -= cellHeight;
            }
        }

        private void OnDrawBorder(RenderContext<GridState> ctx)
        {
            var attr = _options.BorderAttribute;    
            var maxcols = ctx.Viewport.Bounds.Cols - 1;
            var maxrows = ctx.Viewport.Bounds.Rows - 1;
            for (var row = 0; row < ctx.Viewport.Bounds.Rows; row++)
            {
                var horborder = RowHasHorizontalBorder(row);
                if (horborder)
                {
                    ctx.DrawChars(_borderSet[BorderSets.Entries.HORIZONTAL], ctx.Viewport.Bounds.Cols, TvPoint.FromXY(0, row), attr);
                }
                for (var col = 0; col < ctx.Viewport.Bounds.Cols; col++)
                {
                    if (ColHasVerticalBorder(col))
                    {
                        var borderchar = (row, col) switch
                        {
                            (0, 0) => horborder ? _borderSet[BorderSets.Entries.TOPLEFT] : _borderSet[BorderSets.Entries.VERTICAL],
                            var (r, c) when r == 0 && c == maxcols => horborder ? _borderSet[BorderSets.Entries.TOPRIGHT] : _borderSet[BorderSets.Entries.VERTICAL],
                            (0, _) => horborder ? _borderSet[BorderSets.Entries.HORIZONTALDOWN] : _borderSet[BorderSets.Entries.VERTICAL],
                            var (r, c) when c == 0 && r == maxrows => _borderSet[horborder ? BorderSets.Entries.BOTTOMLEFT : BorderSets.Entries.VERTICAL],
                            var (r, c) when c == maxcols && r == maxrows => _borderSet[horborder ? BorderSets.Entries.BOTTOMRIGHT : BorderSets.Entries.VERTICAL],
                            var (r, _) when r == maxrows => _borderSet[horborder ? BorderSets.Entries.HORIZONTALUP : BorderSets.Entries.VERTICAL],
                            (_, 0) => _borderSet[horborder ? BorderSets.Entries.VERTICALLEFT : BorderSets.Entries.VERTICAL],
                            var (_, c) when c == maxcols => _borderSet[horborder ? BorderSets.Entries.VERTICALRIGHT : BorderSets.Entries.VERTICAL],
                            _ => _borderSet[horborder ? BorderSets.Entries.CROSS : BorderSets.Entries.VERTICAL],
                        };
                        ctx.DrawChars(borderchar, 1, TvPoint.FromXY(col, row), attr);
                    }
                }
            }

            bool RowHasHorizontalBorder(int row)
            {
                if (!_options.Border.HasHorizontalBorder) return false;

                if (row == 0 || row == ctx.Viewport.Bounds.Rows - 1) return true;
                var accrow = 0;
                for (var hrow = 0; hrow < _state.Rows; hrow++)
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


        public IGridContainer AtRowCol(int row, int col)
        {
            _currentRow = row;
            _currentCol = col;
            return this;
        }


        public IRowColSpecifier<IGridContainer> At() => this;

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
            TvPoint childDisplacement = entry.ViewportOriginal ? innerViewport.Position  : TvPoint.FromXY(horDespl, verDespl);

            var viewport = alignment switch
            {
                ChildAlignment.None => myViewport.InnerViewport(cellPos + childDisplacement, innerViewport.Bounds, cellBounds),
                ChildAlignment.StretchHorizontal => myViewport.InnerViewport(cellPos + childDisplacement, TvBounds.FromRowsAndCols(cellBounds.Rows, innerViewport.Bounds.Cols), cellBounds),
                ChildAlignment.StretchVertical => myViewport.InnerViewport(cellPos + childDisplacement, TvBounds.FromRowsAndCols(innerViewport.Bounds.Rows, cellBounds.Cols), cellBounds),
                _ => new Viewport(cellPos + childDisplacement, cellBounds, innerViewport.ZIndex, innerViewport.Flow)
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

        IRowSpecifier<IGridContainer> IRowColSpecifier<IGridContainer>.Col(int index)
        {
            _currentCol = index;
            return this;
        }

        IColSpecifier<IGridContainer> IRowColSpecifier<IGridContainer>.Row(int index)
        {
            _currentRow = index;
            return this;
        }

        IGridContainer IColSpecifier<IGridContainer>.Col(int index)
        {
            _currentCol = index;
            return this;
        }

        IGridContainer IRowSpecifier<IGridContainer>.Row(int index)
        {
            _currentRow = index;
            return this;
        }

        public int Count { get => _childs.Count; }
    }
}