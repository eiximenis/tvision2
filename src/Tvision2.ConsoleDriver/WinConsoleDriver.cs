using System;
using Tvision2.ConsoleDriver.Win32;
using Tvision2.Engine.Console;
using Tvision2.Events;

namespace Tvision2.ConsoleDriver
{
    public class WinConsoleDriver : IConsoleDriver
    {
        private const int STDIN = -10;
        private const int STDOUT = -11;
        private const int CP_UTF8 = 65001;
        private readonly IntPtr _hstdin;
        private readonly IntPtr _hstdout;

        private readonly ConsoleDriverOptions _options;

        public (int rows, int cols) GetConsoleWindowSize()
        {
            CONSOLE_SCREEN_BUFFER_INFO csbi;

            ConsoleNative.GetConsoleScreenBufferInfo(_hstdout, out csbi);
            var columns = csbi.srWindow.Right - csbi.srWindow.Left + 1;
            var rows = csbi.srWindow.Bottom - csbi.srWindow.Top + 1;
            return (rows, columns);
        }

        public WinConsoleDriver(ConsoleDriverOptions options)
        {
            _hstdin = ConsoleNative.GetStdHandle(STDIN);
            _hstdout = ConsoleNative.GetStdHandle(STDOUT);
            _options = options;
             ConsoleNative.SetConsoleOutputCP(CP_UTF8);
        }

        public void Init()
        {
            var (rows, cols) = GetConsoleWindowSize();
            var requestedCols = _options.WindowOptions.Cols > 0 ? _options.WindowOptions.Cols : cols;
            var requestedRows = _options.WindowOptions.Rows > 0 ? _options.WindowOptions.Rows : rows;
            var rect = new SMALL_RECT()
            {
                Left = 0,
                Top = 0,
                Bottom = (short)requestedRows,
                Right = (short)requestedCols
            };
            if (_options.WindowOptions.Cols > 0 || _options.WindowOptions.Rows > 0)
            {
                ConsoleNative.SetConsoleWindowInfo(_hstdout, true, ref rect);
            }
            ConsoleNative.SetConsoleScreenBufferSize(_hstdout, new COORD((short)(rect.Right + 1), (short)(rect.Bottom + 1)));
        }

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

        public void SetCursorAt(int x, int y)
        {
            ConsoleNative.SetConsoleCursorPosition(_hstdout, new COORD((short)x, (short)y));
        }
    }
}
