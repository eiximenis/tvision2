using System;
using System.Runtime.InteropServices;
using Tvision2.ConsoleDriver.Win32;
using Tvision2.Core.Colors;
using Tvision2.Core.Render;
using Tvision2.Engine.Console;
using Tvision2.Events;

namespace Tvision2.ConsoleDriver
{
    class Win32StdConsoleDriver : IConsoleDriver
    {
        private const int STDIN = -10;
        private const int STDOUT = -11;
        private const int CP_TOUSE = 65001;     // UTF8
        private readonly IntPtr _hstdin;
        private readonly IntPtr _hstdout;
        private IntPtr _hout;
        private IntPtr _hin;
        private readonly WindowsConsoleDriverOptions _options;
        private readonly Win32StdColorManager _colorManager;
        private CONSOLE_CURSOR_INFO _initialCursorInfo;

        public IColorManager ColorManager => _colorManager;
        public TvBounds ConsoleBounds { get; private set; }

        public TvColor DefaultBackground { get => _options.DefaultBackColor; }

        private TvBounds GetConsoleWindowSize()
        {
            CONSOLE_SCREEN_BUFFER_INFO csbi;

            ConsoleNative.GetConsoleScreenBufferInfo(_hstdout, out csbi);
            var columns = csbi.srWindow.Right - csbi.srWindow.Left + 1;
            var rows = csbi.srWindow.Bottom - csbi.srWindow.Top + 1;
            return TvBounds.FromRowsAndCols(rows, columns);
        }

        public Win32StdConsoleDriver(WindowsConsoleDriverOptions options)
        {
            _colorManager = new Win32StdColorManager();
            _hstdin = ConsoleNative.GetStdHandle(STDIN);
            _hstdout = ConsoleNative.GetStdHandle(STDOUT);
            _hout = IntPtr.Zero;
            _hin = IntPtr.Zero;
            _options = options;
            ConsoleNative.SetConsoleCP(CP_TOUSE);
            ConsoleNative.SetConsoleOutputCP(CP_TOUSE);
            ConsoleNative.GetConsoleMode(_hstdout, out uint dwOutModes);
            ConsoleNative.GetConsoleMode(_hstdin, out uint dwInputModes);

            SetConsoleMode(options, dwOutModes, dwInputModes);
            //ConsoleNative.SetConsoleMode(_hstdin, (uint)(ConsoleInputModes.ENABLE_MOUSE_INPUT | ConsoleInputModes.ENABLE_WINDOW_INPUT | ConsoleInputModes.ENABLE_VIRTUAL_TERMINAL_INPUT));

        }

        private void SetConsoleMode(ConsoleDriverOptions options, uint currentOutModes, uint currentInputModes)
        {

        }

        public void Init()
        {
            var rows = Console.WindowHeight;
            var cols = Console.WindowWidth;
            var requestedCols = _options.WindowOptions.Cols > 0 ? _options.WindowOptions.Cols : cols;
            var requestedRows = _options.WindowOptions.Rows > 0 ? _options.WindowOptions.Rows : rows;
            var rect = new SMALL_RECT()
            {
                Left = 0,
                Top = 0,
                Bottom = (short)requestedRows,
                Right = (short)requestedCols
            };


            var hbuffer = ConsoleNative.CreateConsoleScreenBuffer(AccessMode.GENERIC_READ | AccessMode.GENERIC_WRITE, ShareMode.FILE_SHARE_READ | ShareMode.FILE_SHARE_WRITE, IntPtr.Zero, ScreenBufferFlags.CONSOLE_TEXTMODE_BUFFER, IntPtr.Zero);

            if (hbuffer != HandleNative.INVALID_HANDLE_VALUE)
            {
                _hout = hbuffer;
                _hin = hbuffer;
                ConsoleNative.SetConsoleActiveScreenBuffer(_hout);
            }
            else
            {
                _hout = _hstdout;
                _hin = _hstdin;
            }

            if (_options.WindowOptions.Cols > 0 || _options.WindowOptions.Rows > 0)
            {
                ConsoleNative.SetConsoleWindowInfo(_hout, true, ref rect);
            }

            ConsoleNative.SetConsoleScreenBufferSize(_hout, new COORD((short)(rect.Right + 1), (short)(rect.Bottom + 1)));


            CONSOLE_CURSOR_INFO cursorInfo;
            ConsoleNative.GetConsoleCursorInfo(_hout, out cursorInfo);
            _initialCursorInfo = cursorInfo;

            ConsoleBounds = GetConsoleWindowSize();
        }


        public void End()
        {
            if (_hout != _hstdout)
            {
                ConsoleNative.SetConsoleActiveScreenBuffer(_hstdout);
                HandleNative.CloseHandle(_hout);
            }
        }

        public ITvConsoleEvents ReadEvents()
        {
            ConsoleNative.GetNumberOfConsoleInputEvents(_hstdin, out var numEvents);

            if (numEvents > 0)
            {
                Span<INPUT_RECORD> buffer = stackalloc INPUT_RECORD[(int)numEvents];
                var byteBuf = MemoryMarshal.AsBytes(buffer);
                ConsoleNative.ReadConsoleInput(_hstdin, ref MemoryMarshal.GetReference(byteBuf), numEvents, out var eventsRead);
                return new TvConsoleEvents().Add(buffer);
            }
            else
            {
                return TvConsoleEvents.Empty;
            }
        }

        public void WriteCharacterAt(int x, int y, char character, CharacterAttribute attribute)
        {
            WriteCharactersAt(x, y, 1, character, attribute);
        }

        public void WriteCharactersAt(int x, int y, int count, char character, CharacterAttribute attribute)
        {
            var coord = new COORD((short)x, (short)y);
            var winattr = _colorManager.AttributeToWin32Colors(attribute);
            ConsoleNative.FillConsoleOutputAttribute(_hout, (ushort)winattr, (uint)count, coord, out var numAttrWritten);
            ConsoleNative.FillConsoleOutputCharacter(_hout, (char)character, (uint)count, coord, out var numWritten);
        }


        public void SetCursorAt(int x, int y)
        {
            ConsoleNative.SetConsoleCursorPosition(_hout, new COORD((short)x, (short)y));
        }


        public void SetCursorVisibility(bool isVisible)
        {
            var info = new CONSOLE_CURSOR_INFO(isVisible, _initialCursorInfo.Size);
            ConsoleNative.SetConsoleCursorInfo(_hout, ref info);
        }

        public void ProcessWindowEvent(TvWindowEvent windowEvent)
        {
            var bounds = GetConsoleWindowSize();
            windowEvent.Update(bounds.Cols, bounds.Rows);
            ConsoleBounds = bounds;
        }
    }
}
