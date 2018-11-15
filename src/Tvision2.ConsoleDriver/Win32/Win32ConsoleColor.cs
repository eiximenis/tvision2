using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Colors;

namespace Tvision2.ConsoleDriver.Win32
{
    static class Win32ConsoleColor
    {
        public const ushort FOREGROUND_BLUE = 0x0001;
        public const ushort FOREGROUND_GREEN = 0x0002;
        public const ushort FOREGROUND_RED = 0x0004;
        public const ushort FOREGROUND_INTENSITY = 0x0008;
        public const ushort BACKGROUND_BLUE = 0x0010;
        public const ushort BACKGROUND_GREEN = 0x0020;
        public const ushort BACKGROUND_RED = 0x0040;
        public const ushort BACKGROUND_INTENSITY = 0x0080;
   
        public static ushort OnlyForegroundAttributes(ushort attributes) => (ushort)(attributes & ~(BACKGROUND_BLUE | BACKGROUND_GREEN | BACKGROUND_RED | BACKGROUND_INTENSITY));
        public static ushort OnlyBackgroundAttributes(ushort attributes) => (ushort)(attributes & ~(FOREGROUND_BLUE | FOREGROUND_GREEN | FOREGROUND_RED | FOREGROUND_INTENSITY));

        public static ConsoleColor AttributesToForeConsoleColor(ushort attributes)
        {
            var onlyFore = OnlyForegroundAttributes(attributes);
            switch (onlyFore)
            {
                case 0: return ConsoleColor.Black;
                case FOREGROUND_INTENSITY: return ConsoleColor.Gray;
                case FOREGROUND_BLUE | FOREGROUND_INTENSITY: return ConsoleColor.Blue;
                case FOREGROUND_BLUE | FOREGROUND_GREEN | FOREGROUND_INTENSITY: return ConsoleColor.Cyan;
                case FOREGROUND_BLUE | FOREGROUND_RED | FOREGROUND_INTENSITY: return ConsoleColor.Magenta;
                case FOREGROUND_RED | FOREGROUND_INTENSITY: return ConsoleColor.Red;
                case FOREGROUND_RED | FOREGROUND_GREEN | FOREGROUND_BLUE | FOREGROUND_INTENSITY: return ConsoleColor.White;
                case FOREGROUND_RED | FOREGROUND_GREEN | FOREGROUND_INTENSITY: return ConsoleColor.Yellow;
                case FOREGROUND_GREEN | FOREGROUND_INTENSITY: return ConsoleColor.Green;

                case FOREGROUND_BLUE: return ConsoleColor.DarkBlue;
                case FOREGROUND_BLUE | FOREGROUND_GREEN: return ConsoleColor.DarkCyan;
                case FOREGROUND_BLUE | FOREGROUND_RED: return ConsoleColor.DarkMagenta;
                case FOREGROUND_RED: return ConsoleColor.DarkRed;
                case FOREGROUND_RED | FOREGROUND_GREEN | FOREGROUND_BLUE: return ConsoleColor.Gray;
                case FOREGROUND_RED | FOREGROUND_GREEN: return ConsoleColor.DarkYellow;
                case FOREGROUND_GREEN: return ConsoleColor.DarkGreen;
            }

            return ConsoleColor.Gray;
        }

        public static ConsoleColor AttributesToBackConsoleColor(ushort attributes)
        {
            var onlyBack = OnlyBackgroundAttributes(attributes);
            switch (onlyBack)
            {
                case 0: return ConsoleColor.Black;
                case BACKGROUND_INTENSITY: return ConsoleColor.Gray;
                case BACKGROUND_BLUE | BACKGROUND_INTENSITY: return ConsoleColor.Blue;
                case BACKGROUND_BLUE | BACKGROUND_GREEN | BACKGROUND_INTENSITY: return ConsoleColor.Cyan;
                case BACKGROUND_BLUE | BACKGROUND_RED | BACKGROUND_INTENSITY: return ConsoleColor.Magenta;
                case BACKGROUND_RED | BACKGROUND_INTENSITY: return ConsoleColor.Red;
                case BACKGROUND_RED | BACKGROUND_GREEN | BACKGROUND_BLUE | BACKGROUND_INTENSITY: return ConsoleColor.White;
                case BACKGROUND_RED | BACKGROUND_GREEN | BACKGROUND_INTENSITY: return ConsoleColor.Yellow;
                case BACKGROUND_GREEN | BACKGROUND_INTENSITY: return ConsoleColor.Green;
                case BACKGROUND_BLUE: return ConsoleColor.DarkBlue;
                case BACKGROUND_BLUE | BACKGROUND_GREEN: return ConsoleColor.DarkCyan;
                case BACKGROUND_BLUE | BACKGROUND_RED: return ConsoleColor.DarkMagenta;
                case BACKGROUND_RED: return ConsoleColor.DarkRed;
                case BACKGROUND_RED | BACKGROUND_GREEN | BACKGROUND_BLUE: return ConsoleColor.Gray;
                case BACKGROUND_RED | BACKGROUND_GREEN: return ConsoleColor.DarkYellow;
                case BACKGROUND_GREEN: return ConsoleColor.DarkGreen;
            }

            return ConsoleColor.Gray;
        }

        public static ushort ForeConsoleColorToAttribute(TvisionColor color)
        {
            switch (color)
            {
                case TvisionColor.Black:
                    return (ushort)0;
                case TvisionColor.Blue:
                    return FOREGROUND_BLUE;
                case TvisionColor.Cyan:
                    return FOREGROUND_BLUE;
                case TvisionColor.Green:
                    return FOREGROUND_GREEN;
                case TvisionColor.Magenta:
                    return FOREGROUND_BLUE | FOREGROUND_RED;
                case TvisionColor.Red:
                    return FOREGROUND_RED;
                case TvisionColor.White:
                    return FOREGROUND_RED | FOREGROUND_GREEN | FOREGROUND_BLUE;
                case TvisionColor.Yellow:
                    return FOREGROUND_RED | FOREGROUND_GREEN;
                default:
                    return 0;
            }
        }

        public static ushort BackConsoleColorToAttribute(TvisionColor color)
        {
            switch (color)
            {
                case TvisionColor.Black:
                    return (ushort)0;
                case TvisionColor.Blue:
                    return BACKGROUND_BLUE;
                case TvisionColor.Cyan:
                    return BACKGROUND_BLUE | BACKGROUND_GREEN;
                case TvisionColor.Green:
                    return BACKGROUND_GREEN;
                case TvisionColor.Magenta:
                    return BACKGROUND_BLUE | BACKGROUND_RED;
                case TvisionColor.Red:
                    return BACKGROUND_RED;
                case TvisionColor.White:
                    return BACKGROUND_RED | BACKGROUND_GREEN | BACKGROUND_BLUE;
                case TvisionColor.Yellow:
                    return BACKGROUND_RED | BACKGROUND_GREEN;
                default:
                    return 0;
            }
        }

    }
}
