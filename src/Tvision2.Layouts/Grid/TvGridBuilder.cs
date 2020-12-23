using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
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
        private TvGridOptions _options;

        public TvGridBuilder()
        {
            _name = null;
            _rows = 1;
            _cols = 1;
            _viewport = null;
            _options = new TvGridOptions();
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

        public TvGridBuilder WithOptions(Action<ITvGridOptions> optionsAction)
        {
            optionsAction?.Invoke(_options);
            return this;
        }



        public TvGrid Create()
        {
            var grid = new TvGrid(GridState.FromRowsAndColumns(_rows, _cols), _options, _name ?? $"Grid_{Guid.NewGuid()}");
            if (_viewport != null)
            {
                grid.AsComponent().AddViewport(_viewport);
            }
            return grid;
        }
    }
}
