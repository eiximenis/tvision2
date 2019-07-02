using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core;
using Tvision2.Core.Colors;

namespace Tvision2.ConsoleDriver.Common
{
    class DirectPalette : IPalette
    {
        public bool IsFreezed => true;

        public int MaxColors => -1;

        public ColorMode ColorMode => ColorMode.Direct;

        public bool RedefineColor(int idx, TvColor newColor) => false;
    }
}
