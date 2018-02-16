using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.ConsoleDriver.Win32
{
    class Win32ConsoleColor
    {

        private const ushort FOREGROUND_BLUE = 0x0001;
        private const ushort FOREGROUND_GREEN = 0x0002;
        private const ushort FOREGROUND_RED = 0x0004;
        private const ushort FOREGROUND_INTENSITY = 0x0008;
        private const ushort BACKGROUND_BLUE = 0x0010;
        private const ushort BACKGROUND_GREEN = 0x0020;
        private const ushort BACKGROUND_RED = 0x0040;
        private const ushort BACKGROUND_INTENSITY = 0x0080;


        private readonly IntPtr _hstdout;
        private readonly short _oldAttributes;

        public Win32ConsoleColor(IntPtr handle, ConsoleColor? forecolor) : this(handle, forecolor, null) { }
        public Win32ConsoleColor(IntPtr handle, ConsoleColor? forecolor, ConsoleColor? backColor)
        {
            _hstdout = handle;
            ConsoleNative.GetConsoleScreenBufferInfo(_hstdout, out CONSOLE_SCREEN_BUFFER_INFO info);
            _oldAttributes = info.wAttributes;
            var attributes = forecolor.HasValue ? ForeConsoleColorToAttribute(forecolor.Value) : (ushort)(_oldAttributes & (FOREGROUND_BLUE | FOREGROUND_GREEN | FOREGROUND_RED | FOREGROUND_INTENSITY));
            attributes |= backColor.HasValue ? BackConsoleColorToAttribute(backColor.Value) : (ushort)(_oldAttributes & (BACKGROUND_BLUE | BACKGROUND_GREEN | BACKGROUND_RED | BACKGROUND_INTENSITY));
            ConsoleNative.SetConsoleTextAttribute(_hstdout, attributes);
        }

        public static ushort OnlyForegroundAttributes(ushort attributes) => (ushort)(attributes & ~(BACKGROUND_BLUE | BACKGROUND_GREEN | BACKGROUND_RED | BACKGROUND_INTENSITY));
        public static ushort OnlyBackgroundAttributes(ushort attributes) => (ushort)(attributes & ~(FOREGROUND_BLUE | FOREGROUND_GREEN | FOREGROUND_RED | FOREGROUND_INTENSITY));

        public static Win32ConsoleColor None(IntPtr handle) => new Win32ConsoleColor(handle, forecolor: null, backColor: null);

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

        public void Dispose()
        {
            ConsoleNative.SetConsoleTextAttribute(_hstdout, (ushort)_oldAttributes);
        }

        public static ushort ForeConsoleColorToAttribute(ConsoleColor color)
        {
            switch (color)
            {
                case ConsoleColor.Black:
                    return (ushort)0;
                case ConsoleColor.Blue:
                    return FOREGROUND_BLUE | FOREGROUND_INTENSITY;
                case ConsoleColor.Cyan:
                    return FOREGROUND_BLUE | FOREGROUND_GREEN | FOREGROUND_INTENSITY;
                case ConsoleColor.Green:
                    return FOREGROUND_GREEN | FOREGROUND_INTENSITY;
                case ConsoleColor.Magenta:
                    return FOREGROUND_BLUE | FOREGROUND_RED | FOREGROUND_INTENSITY;
                case ConsoleColor.Red:
                    return FOREGROUND_RED | FOREGROUND_INTENSITY;
                case ConsoleColor.White:
                    return FOREGROUND_RED | FOREGROUND_GREEN | FOREGROUND_BLUE | FOREGROUND_INTENSITY;
                case ConsoleColor.Yellow:
                    return FOREGROUND_RED | FOREGROUND_GREEN | FOREGROUND_INTENSITY;
                case ConsoleColor.DarkBlue:
                    return FOREGROUND_BLUE;
                case ConsoleColor.DarkCyan:
                    return FOREGROUND_BLUE | FOREGROUND_GREEN;
                case ConsoleColor.DarkGray:
                    return FOREGROUND_RED | FOREGROUND_GREEN | FOREGROUND_BLUE;
                case ConsoleColor.DarkGreen:
                    return FOREGROUND_GREEN;
                case ConsoleColor.DarkMagenta:
                    return FOREGROUND_BLUE | FOREGROUND_RED;
                case ConsoleColor.DarkRed:
                    return FOREGROUND_RED;
                case ConsoleColor.DarkYellow:
                    return FOREGROUND_RED | FOREGROUND_GREEN;
                case ConsoleColor.Gray:
                    return FOREGROUND_INTENSITY;
                default:
                    return 0;
            }
        }

        public static ushort BackConsoleColorToAttribute(ConsoleColor color)
        {
            switch (color)
            {
                case ConsoleColor.Black:
                    return (ushort)0;
                case ConsoleColor.Blue:
                    return BACKGROUND_BLUE | BACKGROUND_INTENSITY;
                case ConsoleColor.Cyan:
                    return BACKGROUND_BLUE | BACKGROUND_GREEN | BACKGROUND_INTENSITY;
                case ConsoleColor.Green:
                    return BACKGROUND_GREEN | BACKGROUND_INTENSITY;
                case ConsoleColor.Magenta:
                    return BACKGROUND_BLUE | BACKGROUND_RED | BACKGROUND_INTENSITY;
                case ConsoleColor.Red:
                    return BACKGROUND_RED | BACKGROUND_INTENSITY;
                case ConsoleColor.White:
                    return BACKGROUND_RED | BACKGROUND_GREEN | BACKGROUND_BLUE | BACKGROUND_INTENSITY;
                case ConsoleColor.Yellow:
                    return BACKGROUND_RED | BACKGROUND_GREEN | BACKGROUND_INTENSITY;
                case ConsoleColor.DarkBlue:
                    return BACKGROUND_BLUE;
                case ConsoleColor.DarkCyan:
                    return BACKGROUND_BLUE | BACKGROUND_GREEN;
                case ConsoleColor.DarkGray:
                    return BACKGROUND_RED | BACKGROUND_GREEN | BACKGROUND_BLUE;
                case ConsoleColor.DarkGreen:
                    return BACKGROUND_GREEN;
                case ConsoleColor.DarkMagenta:
                    return BACKGROUND_BLUE | BACKGROUND_RED;
                case ConsoleColor.DarkRed:
                    return BACKGROUND_RED;
                case ConsoleColor.DarkYellow:
                    return BACKGROUND_RED | BACKGROUND_GREEN;
                case ConsoleColor.Gray:
                    return BACKGROUND_INTENSITY;
                default:
                    return 0;
            }
        }

    }
}
