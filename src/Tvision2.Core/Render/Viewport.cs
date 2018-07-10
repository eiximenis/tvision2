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

        public IViewport Clone()
        {
            return new Viewport(Position, Columns, Rows, Clipping) { ZIndex = ZIndex };
        }

        public bool Intersects(IViewport another)
        {
            var otherPos = another.Position;

            var x1 = Position.Left;
            var y1 = Position.Top;
            var x2 = x1 + Columns;
            var y2 = y1 + Rows;

            var x1o = otherPos.Left;
            var y1o = otherPos.Top;
            var x2o = x1o + another.Columns;
            var y2o = y1o + another.Rows;

            return (x2o >= x1 && x1o <= x2) && (y2o >= y1 && y1o <= y2);
        }
    }
}
