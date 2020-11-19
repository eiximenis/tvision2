using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Text;
using Tvision2.ConsoleDriver.Ansi;
using Tvision2.ConsoleDriver.Win32;
using Tvision2.Core.Colors;
using Tvision2.Core.Render;
using Tvision2.Engine.Console;
using Tvision2.Events;

namespace Tvision2.ConsoleDriver
{
    class Win32AnsiSeqConsoleDriver : IConsoleDriver
    {
        private const int STDIN = -10;
        private const int STDOUT = -11;
        private const int CP_TOUSE = 65001;     // UTF8
        private readonly IntPtr _hstdin;
        private readonly IntPtr _hstdout;
        public bool SupportsAnsi { get; }
        private readonly AnsiColorManager _colorManager;
        private readonly WindowsConsoleDriverOptions _options;

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
        

        public Win32AnsiSeqConsoleDriver(WindowsConsoleDriverOptions options)
        {
            _hstdin = ConsoleNative.GetStdHandle(STDIN);
            _hstdout = ConsoleNative.GetStdHandle(STDOUT);
            _colorManager = new AnsiColorManager(options.PaletteOptions);
            _options = options;
        }


        private void SetConsoleMode(ConsoleDriverOptions options)
        {
            ConsoleNative.GetConsoleMode(_hstdout, out uint dwOutModes);
            ConsoleNative.GetConsoleMode(_hstdin, out uint dwInputModes);

            if (options.MouseStatusDesired == MouseStatus.Enabled)
            {

                ConsoleNative.SetConsoleMode(_hstdin, (uint)ConsoleInputModes.ENABLE_EXTENDED_FLAGS | (uint)ConsoleInputModes.ENABLE_MOUSE_INPUT | (uint)ConsoleInputModes.ENABLE_WINDOW_INPUT);
            }

        }

        public void Init()
        {
            SetConsoleMode(_options);
            ConsoleBounds = GetConsoleWindowSize();

            /*
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
            */


            Console.Write("\x1b[2J");
        }

        private readonly Win32EventsProcessor _eventsProcessor = new Win32EventsProcessor();
        public ITvConsoleEvents ReadEvents()
        {
            ConsoleNative.GetNumberOfConsoleInputEvents(_hstdin, out var numEvents);

            if (numEvents > 0)
            {
                Span<INPUT_RECORD> buffer = stackalloc INPUT_RECORD[(int)numEvents];
                var byteBuf = MemoryMarshal.AsBytes(buffer);
                ConsoleNative.ReadConsoleInput(_hstdin, ref MemoryMarshal.GetReference(byteBuf), numEvents, out var eventsRead);
                return _eventsProcessor.AddWin32Events(new TvConsoleEvents(), buffer);

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
            var sb = new StringBuilder();
            sb.Append(_colorManager.GetCursorSequence(x, y));
            sb.Append(_colorManager.GetAttributeSequence(attribute));
            sb.Append(character, count);
            Console.Write(sb.ToString());
        }


        public void SetCursorAt(int x, int y)
        {
            Console.Write(_colorManager.GetCursorSequence(x, y));
        }

        public void SetCursorVisibility(bool isVisible)
        {
            if (isVisible)
            {
                Console.Write(AnsiEscapeSequences.DECTCEM_VISIBLE);
            }
            else
            {
                Console.Write(AnsiEscapeSequences.DECTCEM_HIDDEN);
            }
        }

        public void ProcessWindowEvent(TvWindowEvent windowEvent)
        {
            var bounds = GetConsoleWindowSize();
            windowEvent.Update(bounds.Cols, bounds.Rows);
            ConsoleBounds = bounds;
        }
    }
}
