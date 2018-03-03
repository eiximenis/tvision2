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

        public bool Grow(int cols, int rows)
        {
            return true;
        }

        public BoxModel(TvPoint point, int cols, int rows = 1)
        {
            Position = point;
            Clipping = ClippingMode.Clip;
            Columns = cols;
            Rows = rows;
            ZIndex = 0;
        }
    }
}
