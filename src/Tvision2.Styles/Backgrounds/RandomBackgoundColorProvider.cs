using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Colors;
using Tvision2.Core.Render;

namespace Tvision2.Styles.Backgrounds
{
    public class RandomBackgoundColorProvider : IBackgroundProvider
    {
        public bool IsFixedBackgroundColor => false;

        public TvColor GetColorFor(int row, int col, TvBounds bounds)
        {
            var rnd = new Random();
            var red = (byte)rnd.Next(0, 256);
            var green = (byte)rnd.Next(0, 256);
            var blue = (byte)rnd.Next(0, 256);
          
            return TvColor.FromRGB(red, green, blue);
        }
    }
}
