using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Xml.Serialization;
using Tvision2.Core.Components;
using Tvision2.Core.Engine;
using Tvision2.Core.Render;

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
        public string Name { get; }
        private bool _isMounted;
        private readonly List<TvComponent> _pendingAdds;
        private ComponentTree _tree;
        public TvComponent AsComponent() => _component;
        public static TvGridBuilder With() => new TvGridBuilder();
        private readonly TvGridOptions _options;

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
            if (_options.HasBorder)
            {
                _component.AddDrawer(ctx => OnDrawBorder(ctx));
            }

            _component.Metadata.ViewportChanged += OnViewportChanged;
            _childs = new TvGridComponentTree();
        }

        private void OnDrawBorder(RenderContext<GridState> ctx)
        {
            int i = 0;
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
            TvBounds cellBounds = _options.HasBorder ? TvBounds.FromRowsAndCols(cellHeight - 2, cellWidth - 2) : TvBounds.FromRowsAndCols(cellHeight, cellWidth);
            TvPoint childDisplacement = entry.ViewportOriginal ? childViewport.Position : (_options.HasBorder ? TvPoint.FromXY(1, 1) : TvPoint.Zero);


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
