using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Tvision2.Core.Components;
using Tvision2.Core.Engine;
using Tvision2.Core.Render;

namespace Tvision2.Layouts.Grid
{
    public class TvGrid : ITvContainer, IComponentsCollection
    {
        private readonly TvComponent<GridState> _component;
        private readonly TvGridComponentTree _ui;
        private readonly GridState _state;
        public string Name { get; }

        private bool _isMounted;
        private readonly List<TvComponent> _pendingAdds;
        private ComponentTree _tree;

        public TvComponent AsComponent() => _component;

        public TvGrid() : this(new GridState(1, 1), null)
        {
        }
        public TvGrid(GridState state, string name = null)
        {
            _pendingAdds = new List<TvComponent>();
            _isMounted = false;
            _state = state;
            _component = new TvComponent<GridState>(_state, name ?? $"TvGrid_{Guid.NewGuid()}",
                cfg =>
                {
                    cfg.WhenChildMounted(ctx => ResizeRowsAndCols());
                    cfg.WhenChildUnmounted(ctx => ResizeRowsAndCols());
                    cfg.WhenComponentMounted(ctx => OnComponentMounted(ctx));
                });
            Name = _component.Name;
            _component.Metadata.ViewportChanged += OnViewportChanged;
            _ui = new TvGridComponentTree();
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


        public IComponentsCollection At(int row, int col)
        {
            _ui.CurrentRow = row;
            _ui.CurrentColumn = col;
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

            foreach (var kvpChild in _ui.Values)
            {
                var ctrRow = kvpChild.Key.Row;
                var ctrCol = kvpChild.Key.Column;
                var child = kvpChild.Value;
                var viewport = CalculateViewportFor(myViewport, ctrRow, ctrCol, cellHeight, cellWidth);
                child.UpdateViewport(viewport, addIfNotExists: true);
            }

        }

        private IViewport CalculateViewportFor(IViewport myViewport, int ctrRow, int ctrCol, int cellHeight, int cellWidth)
        {
            var startCol = ctrCol * cellWidth;
            var startRow = ctrRow * cellHeight;

            return myViewport.InnerViewport(TvPoint.FromXY(startCol, startRow), TvBounds.FromRowsAndCols(cellHeight, cellWidth));

        }


        private void DoPendingAdd(TvComponent cmpToAdd)
        {
            _tree.AddAsChild(cmpToAdd, AsComponent(), cfg => cfg.DoNotNotifyParentOnAdd());
        }

        void IComponentsCollection.Add(TvComponent child)
        {
            _ui.Add(child);
            if (_isMounted)
            {
                _tree.AddAsChild(child, AsComponent());
            }
            else
            {
                _pendingAdds.Add(child);
            }
        }

        bool IComponentsCollection.Remove(TvComponent component) => _ui.Remove(component);

        void IComponentsCollection.Clear() => _ui.Clear();

        int IComponentsCollection.Count => _ui.Count;

        IEnumerator<TvComponent> IEnumerable<TvComponent>.GetEnumerator() => _ui.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _ui.GetEnumerator();

    }
}
