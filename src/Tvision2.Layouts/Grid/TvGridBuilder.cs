using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tvision2.Core.Render;

namespace Tvision2.Layouts.Grid
{
    public class TvGridBuilder
    {
        private int _rows;
        private int _cols;
        private string _name;
        private IViewport _viewport;
        private ChildAlignment _alignment;

        public TvGridBuilder()
        {
            _name = null;
            _rows = 1;
            _cols = 1;
            _viewport = null;
            _alignment = ChildAlignment.None;
        }

        public TvGridBuilder Rows(int rows)
        {
            _rows = rows;
            return this;
        }

        public TvGridBuilder Columns(int columns)
        {
            _cols = columns;
            return this;
        }

        public TvGridBuilder Name (string name)
        {
            _name = name;
            return this;
        }

        public TvGridBuilder Viewport(IViewport viewportToUse)
        {
            _viewport = viewportToUse;
            return this;
        }

        public TvGridBuilder AlignChilds(ChildAlignment alignment)
        {
            _alignment = alignment;
            return this;
        }

        public TvGrid Create()
        {
            var grid = new TvGrid(GridState.FromRowsAndColumns(_rows, _cols), _name ?? $"Grid_{Guid.NewGuid()}", _alignment);
            if (_viewport != null)
            {
                grid.AsComponent().AddViewport(_viewport);
            }
            return grid;
        }
    }
}
