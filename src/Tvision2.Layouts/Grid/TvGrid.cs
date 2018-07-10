using System.Collections.Generic;
using Tvision2.Core.Components;
using Tvision2.Core.Engine;
using Tvision2.Core.Render;

namespace Tvision2.Layouts.Grid
{
    public class TvGrid
    {
        private readonly TvComponent<GridState> _component;
        private readonly ComponentTree _root;
        private readonly Dictionary<(int Row, int Column), TvContainer> _containers;
        private readonly GridState _state;
        public TvComponent AsComponent() => _component;

        public TvGrid(ComponentTree root) : this(root, new GridState(1, 1), null)
        {
        }
        public TvGrid(ComponentTree root, GridState state, string name = null)
        {
            _state = state;
            _component = new TvComponent<GridState>(_state, name);
            _containers = new Dictionary<(int, int), TvContainer>();
            _root = root;
            ResizeRowsAndCols();
        }

        public void AddChild(TvComponent child, int row, int col)
        {
            var containerToUse = _containers.TryGetValue((row, col), out TvContainer value) ? value : null;
            if (containerToUse == null)
            {
                containerToUse = new TvContainer(_root, $"{_component.Name}_{row}_{col}");
                _containers.Add((row, col), containerToUse);
                _root.Add(containerToUse.AsComponent());
            }
            containerToUse.AddChild(child);
            ResizeRowsAndCols();
        }

        private void ResizeRowsAndCols()
        {
            var myViewport = AsComponent().Viewport;
            if (myViewport == null)
            {
                return;
            }

            var cellHeight = myViewport.Rows / _state.Rows;
            var cellWidth = myViewport.Columns / _state.Cols;

            foreach (var kvpContainer in _containers)
            {
                var ctrRow = kvpContainer.Key.Row;
                var ctrCol = kvpContainer.Key.Column;
                var ctr = kvpContainer.Value;
                var viewport = CalculateViewportFor(myViewport, ctrRow, ctrCol, cellHeight, cellWidth);
                ctr.AsComponent().UpdateViewport(viewport, addIfNotExists: true);
            }

        }

        private IViewport CalculateViewportFor(IViewport myViewport, int ctrRow, int ctrCol, int cellHeight, int cellWidth)
        {
            var startCol = ctrCol * cellWidth;
            var startRow = ctrRow * cellHeight;

            return myViewport.Inner(new TvPoint(startRow, startCol), cellWidth, cellHeight);

        }
    }
}
