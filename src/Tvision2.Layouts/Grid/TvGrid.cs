using System;
using System.Collections.Generic;
using Tvision2.Core.Components;
using Tvision2.Core.Engine;
using Tvision2.Core.Render;

namespace Tvision2.Layouts.Grid
{
    public class TvGrid : ITvContainer
    {
        private readonly TvComponent<GridState> _component;
        private readonly IComponentTree _root;
        private readonly TvGridComponentTree _ui;

        private readonly GridState _state;
        public string Name { get; }
        public TvComponent AsComponent() => _component;

        public TvGrid(IComponentTree root) : this(root, new GridState(1, 1), null)
        {
        }
        public TvGrid(IComponentTree root, GridState state, string name = null)
        {
            _state = state;
            _component = new TvComponent<GridState>(_state, name ?? $"TvGrid_{Guid.NewGuid()}");
            Name = _component.Name;
            _component.Metadata.ViewportChanged += OnViewportChanged;
            _root = root;
            _ui = new TvGridComponentTree(_root);
            _ui.ComponentAdded += OnComponentAdded;
            _ui.ComponentRemoved += OnComponentRemoved;
            _root.ComponentRemoved += OnRootRemoved;
        }

        private void OnRootRemoved(object sender, TreeUpdatedEventArgs e)
        {
            if (e.ComponentMetadata == _component.Metadata)
            {
                _root.ComponentRemoved -= OnRootRemoved;
                Clear();
            }
        }

        public void Clear()
        {
            _ui.Clear();
        }

        private void OnComponentRemoved(object sender, TreeUpdatedEventArgs e)
        {
            ResizeRowsAndCols();
        }

        private void OnComponentAdded(object sender, TreeUpdatedEventArgs e)
        {
            ResizeRowsAndCols();
        }

        private void OnViewportChanged(object sender, ViewportUpdatedEventArgs e)
        {
            ResizeRowsAndCols();
        }


        public IComponentTree At(int row, int col)
        {
            _ui.CurrentRow = row;
            _ui.CurrentColumn = col;
            return _ui;
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

            return myViewport.InnerViewport(new TvPoint(startCol, startRow), new TvBounds(cellHeight, cellWidth));

        }
    }
}
