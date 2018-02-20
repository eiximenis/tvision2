using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.ConsoleDriver.Win32;
using Tvision2.Events;

namespace Tvision2.ConsoleDriver
{
    public class WinConsoleDriver : IConsoleDriver
    {
        private const int STDIN = -10;
        private const int STDOUT = -11;
        private readonly IntPtr _hstdin;
        private readonly IntPtr _hstdout;
        public WinConsoleDriver()
        {
            _hstdin = ConsoleNative.GetStdHandle(STDIN);
            _hstdout = ConsoleNative.GetStdHandle(STDOUT);
        }

        public void Init() {}
        public TvConsoleEvents ReadEvents()
        {
            ConsoleNative.GetNumberOfConsoleInputEvents(_hstdin, out uint numEvents);

            if (numEvents > 0)
            {
                var buffer = new INPUT_RECORD[numEvents];
                ConsoleNative.ReadConsoleInput(_hstdin, buffer, (uint)buffer.Length, out uint eventsRead);
                return new TvConsoleEvents().Add(buffer);
            }
            else
            {
                return TvConsoleEvents.Empty;
            }
        }

        public void WriteCharacterAt(int x, int y, char character)
        {
            ConsoleNative.FillConsoleOutputCharacter(_hstdout, character, (uint)1, new COORD((short)x, (short)y), out var numWritten);
        }

        public void WriteCharacterAt(int x, int y, char character, ConsoleColor foreColor, ConsoleColor backColor)
        {
            var coord = new COORD((short)x, (short)y);

            var attribute = (ushort)(Win32ConsoleColor.ForeConsoleColorToAttribute(foreColor) | Win32ConsoleColor.BackConsoleColorToAttribute(backColor));

            ConsoleNative.FillConsoleOutputAttribute(_hstdout, attribute, (uint)1, coord, out var numAttrWritten);
            ConsoleNative.FillConsoleOutputCharacter(_hstdout, character, (uint)1, coord, out var numWritten);

        }
    }
}
