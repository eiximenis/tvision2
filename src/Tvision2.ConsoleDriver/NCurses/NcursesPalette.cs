using Tvision2.Core;
using Tvision2.Core.Colors;
using Unix.Terminal;

namespace Tvision2.ConsoleDriver.NCurses
{
    class NcursesPalette : IPalette
    {

        public bool IsFreezed { get; private set; }

        public int MaxColors { get; private set; }

        public ColorMode ColorMode { get; private set; }

        public NcursesPalette()
        {
            IsFreezed = true;
            MaxColors = 0;
            ColorMode = ColorMode.Palettized;
        }

        public bool RedefineColor(int idx, TvColor newColor)
        {
            if (IsFreezed || !newColor.IsRgb)
            {
                return false;
            }

            var (red, green, blue) = newColor.Rgb;
            Curses.InitColor((short)idx, (short)red, (short)green, (short)blue);
            return true;
        }

        internal void Init()
        {
            if (Curses.HasColors)
            {
                Curses.StartColor();
                IsFreezed = !Curses.CanChangeColor();
                MaxColors = Curses.Colors;
            }
            else
            {
                ColorMode = ColorMode.NoColors;
                IsFreezed = true;
                MaxColors = 1;
            }
        }
    }
}