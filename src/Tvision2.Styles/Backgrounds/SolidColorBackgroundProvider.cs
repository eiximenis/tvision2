using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Colors;
using Tvision2.Core.Render;

namespace Tvision2.Styles.Backgrounds
{
    public class SolidColorBackgroundProvider : IBackgroundProvider
    {
        public static SolidColorBackgroundProvider BlackBackground = new SolidColorBackgroundProvider(TvColor.Black);

        private readonly TvColor _color;
        public SolidColorBackgroundProvider(TvColor color) => _color = color;

        public bool IsFixedBackgroundColor => true;

        public TvColor GetColorFor(int row, int col, TvBounds bounds) => _color;
    }
}
