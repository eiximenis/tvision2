using System;
using Tvision2.ConsoleDriver.Win32;
using Tvision2.Core.Colors;
using Tvision2.Engine.Console;
using Tvision2.Events;

namespace Tvision2.ConsoleDriver
{
    public class Win32ConsoleDriverFactory
    {
        private const int STDIN = -10;
        private const int STDOUT = -11;
        private const int CP_TOUSE = 65001;     // UTF8
        private readonly IntPtr _hstdin;
        private readonly IntPtr _hstdout;

        public bool SupportsAnsi { get; }
        public bool AnsiRequested { get; }
        private readonly WindowsConsoleDriverOptions _options;
        private CONSOLE_CURSOR_INFO _initialCursorInfo;

        public IConsoleDriver CreateWindowsDriver()
        {
            return SupportsAnsi && AnsiRequested
                ? (IConsoleDriver)new Win32AnsiSeqConsoleDriver(_options)
                : (IConsoleDriver)new Win32StdConsoleDriver(_options);
        }


        public Win32ConsoleDriverFactory(WindowsConsoleDriverOptions options)
        {
            _hstdin = ConsoleNative.GetStdHandle(STDIN);
            _hstdout = ConsoleNative.GetStdHandle(STDOUT);
            _options = options;
            ConsoleNative.SetConsoleCP(CP_TOUSE);
            ConsoleNative.SetConsoleOutputCP(CP_TOUSE);
            ConsoleNative.GetConsoleMode(_hstdout, out uint dwOutModes);
            ConsoleNative.GetConsoleMode(_hstdin, out uint dwInputModes);

            if (options.UseAnsiSequences)
            {
                AnsiRequested = true;
                dwOutModes |= (uint)ConsoleOutputModes.ENABLE_VIRTUAL_TERMINAL_PROCESSING;
                SupportsAnsi = ConsoleNative.SetConsoleMode(_hstdout, dwOutModes);
            }

            // SetConsoleMode(options, dwOutModes, dwInputModes);
            //ConsoleNative.SetConsoleMode(_hstdin, (uint)(ConsoleInputModes.ENABLE_MOUSE_INPUT | ConsoleInputModes.ENABLE_WINDOW_INPUT | ConsoleInputModes.ENABLE_VIRTUAL_TERMINAL_INPUT));

        }
    }
}
