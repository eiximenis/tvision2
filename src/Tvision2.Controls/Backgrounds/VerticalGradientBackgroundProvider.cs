using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Colors;
using Tvision2.Core.Render;

namespace Tvision2.Controls.Backgrounds
{
    public class VerticalGradientBackgroundProvider : IBackgroundProvider
    {
        private readonly TvColor _from;
        private readonly TvColor _to;

        public bool IsFixedBackgroundColor => false;

        public VerticalGradientBackgroundProvider(TvColor from, TvColor to)
        {
            _from = from;
            _to = to;
        }

        public TvColor GetColorFor(int row, int col, TvBounds bounds)
        {
            if (bounds.Cols <=0)
            {
                return _from;
            }
            var cols = bounds.Cols;
            var rgb = _from.Rgb;
            var (dr, dg, db) = _from.Diff(_to);
            dr /= cols;
            dg /= cols;
            db /= cols;


            return TvColor.FromRGB((byte)(rgb.red - (dr * col)), (byte)(rgb.green - (dg * col)), (byte)(rgb.blue - (db * col)));
        }

    }
}
