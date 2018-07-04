using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Core.Render
{
    public class Viewport : IViewport
    {
        public TvPoint Position { get; }

        public int ZIndex { get; set; }

        public int Columns { get; }

        public int Rows { get; }

        public ClippingMode Clipping { get; }

        public Viewport(TvPoint point, int cols, int rows, ClippingMode clipping)
        {
            Position = point;
            Clipping = clipping;
            Columns = cols;
            Rows = rows;
            ZIndex = 0;
        }

        public IViewport ResizeTo(int cols, int rows) => new Viewport(Position, cols, rows, Clipping);
        public IViewport Grow(int ncols, int nrows) => new Viewport(Position, Columns + ncols, Rows + nrows, Clipping);

        public IViewport MoveTo(TvPoint newPos) => new Viewport(newPos, Columns, Rows, Clipping);

        public IViewport Translate(TvPoint translation) => new Viewport(Position + translation, Columns, Rows, Clipping);

        public Viewport(TvPoint point, int cols, int rows = 1) : this(point, cols, rows, ClippingMode.Clip) { }
    }
}
