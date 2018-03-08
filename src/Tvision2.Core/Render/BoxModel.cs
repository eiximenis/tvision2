using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Core.Render
{
    public class BoxModel : IBoxModel
    {
        public TvPoint Position { get; }

        public int ZIndex { get; set; }

        public int Columns { get; }

        public int Rows { get; }

        public ClippingMode Clipping { get; }

        public IBoxModel ResizeUp(int cols, int rows)
        {
            var ncols = cols > Columns ? cols : Columns;
            var nrows = rows > Rows ? rows : Rows;

            return new BoxModel(Position, ncols, nrows);
        }

        public IBoxModel Translate(TvPoint newPos) => new BoxModel(newPos, Columns, Rows);

        public BoxModel(TvPoint point, int cols, int rows, ClippingMode clipping)
        {
            Position = point;
            Clipping = clipping;
            Columns = cols;
            Rows = rows;
            ZIndex = 0;
        }

        public BoxModel(TvPoint point, int cols, int rows = 1) : this(point, cols, rows, ClippingMode.Clip) { }
    }
}
