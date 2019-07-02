using Tvision2.Core;
using Tvision2.Core.Colors;

namespace Tvision2.ConsoleDriver.Win32
{
    internal class Win32StdPalette : IPalette
    {
        public bool IsFreezed => true;

        public int MaxColors => TvColorNames.StandardColorsCount;

        public ColorMode ColorMode => ColorMode.Basic;

        public bool RedefineColor(int idx, TvColor newColor) => false;
    }
}