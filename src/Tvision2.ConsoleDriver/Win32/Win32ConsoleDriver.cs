using System;
using Tvision2.ConsoleDriver.Win32;
using Tvision2.Core.Colors;
using Tvision2.Engine.Console;
using Tvision2.Events;

namespace Tvision2.ConsoleDriver
{
    public class Win32ConsoleDriver : IConsoleDriver
    {
        private const int STDIN = -10;
        private const int STDOUT = -11;
        private const int CP_TOUSE = 65001;     // UTF8
        private readonly IntPtr _hstdin;
        private readonly IntPtr _hstdout;
        public bool SupportsVt100 { get; }
        private IWindowsColorManager _colorManager;
        private readonly WindowsConsoleDriverOptions _options;
        private CONSOLE_CURSOR_INFO _initialCursorInfo;

        public (int rows, int cols) GetConsoleWindowSize()
        {
            CONSOLE_SCREEN_BUFFER_INFO csbi;

            ConsoleNative.GetConsoleScreenBufferInfo(_hstdout, out csbi);
            var columns = csbi.srWindow.Right - csbi.srWindow.Left + 1;
            var rows = csbi.srWindow.Bottom - csbi.srWindow.Top + 1;
            return (rows, columns);
        }

        internal void AttachColorManager(IWindowsColorManager colorManager) => _colorManager = colorManager;

        public Win32ConsoleDriver(WindowsConsoleDriverOptions options)
        {
            _hstdin = ConsoleNative.GetStdHandle(STDIN);
            _hstdout = ConsoleNative.GetStdHandle(STDOUT);
            _options = options;
            ConsoleNative.SetConsoleCP(CP_TOUSE);
            ConsoleNative.SetConsoleOutputCP(CP_TOUSE);
            ConsoleNative.GetConsoleMode(_hstdout, out uint dwOutModes);
            ConsoleNative.GetConsoleMode(_hstdin, out uint dwInputModes);
            SetConsoleMode(options, dwOutModes, dwInputModes);
            //ConsoleNative.SetConsoleMode(_hstdin, (uint)(ConsoleInputModes.ENABLE_MOUSE_INPUT | ConsoleInputModes.ENABLE_WINDOW_INPUT | ConsoleInputModes.ENABLE_VIRTUAL_TERMINAL_INPUT));
            dwOutModes |= (uint)ConsoleOutputModes.ENABLE_VIRTUAL_TERMINAL_PROCESSING;
            SupportsVt100 = ConsoleNative.SetConsoleMode(_hstdout, dwOutModes);
        }

        private void SetConsoleMode(ConsoleDriverOptions options, uint currentOutModes, uint currentInputModes)
        {

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

            CONSOLE_CURSOR_INFO cursorInfo;
            ConsoleNative.GetConsoleCursorInfo(_hstdout, out cursorInfo);
            _initialCursorInfo = cursorInfo;
        }

        public TvConsoleEvents ReadEvents()
        {
            ConsoleNative.GetNumberOfConsoleInputEvents(_hstdin, out var numEvents);

            if (numEvents > 0)
            {
                var buffer = new INPUT_RECORD[numEvents];
                ConsoleNative.ReadConsoleInput(_hstdin, buffer, (uint)buffer.Length, out var eventsRead);
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

        public void WriteCharacterAt(int x, int y, char character, CharacterAttribute attribute)
        {
            WriteCharactersAt(x, y, 1, character, attribute);
        }

        public void WriteCharactersAt(int x, int y, int count, char character, CharacterAttribute attribute)
        {
            var coord = new COORD((short)x, (short)y);
            var winattr = _colorManager.AttributeToWin32Colors(attribute);
            ConsoleNative.FillConsoleOutputAttribute(_hstdout, (ushort)winattr, (uint)count, coord, out var numAttrWritten);
            ConsoleNative.FillConsoleOutputCharacter(_hstdout, (char)character, (uint)count, coord, out var numWritten);
        }


        public void SetCursorAt(int x, int y)
        {
            ConsoleNative.SetConsoleCursorPosition(_hstdout, new COORD((short)x, (short)y));
        }


        public void SetCursorVisibility(bool isVisible)
        {
            var info = new CONSOLE_CURSOR_INFO(isVisible, _initialCursorInfo.Size);
            ConsoleNative.SetConsoleCursorInfo(_hstdout, ref info);
        }
    }
}
