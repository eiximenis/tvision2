using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Render;

namespace Tvision2.Controls
{
    public class MutableViewport : IViewport
    {
        private IViewport _viewport;
        public MutableViewport(IViewport viewport) => _viewport = viewport;

        public IViewport Viewport => _viewport;

        public TvPoint Position => _viewport.Position;

        public int ZIndex => _viewport.ZIndex;

        public int Columns => _viewport.Columns;

        public int Rows => _viewport.Rows;

        public ClippingMode Clipping => _viewport.Clipping;

        public IViewport ResizeTo(int cols, int rows)
        {
            _viewport = _viewport.ResizeTo(cols, rows);
            return this;
        }
        public IViewport Grow(int ncols, int nrows)
        {
            _viewport = _viewport.Grow(ncols, nrows);
            return this;
        }
        public IViewport MoveTo(TvPoint newPos)
        {
            _viewport = _viewport.MoveTo(newPos);
            return this;
        }

        public IViewport Translate(TvPoint translation)
        {
            _viewport = _viewport.Translate(translation);
            return this;
        }
    }
}
