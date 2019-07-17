using Tvision2.ConsoleDriver.Common;

namespace Tvision2.ConsoleDriver.Win32
{
    public class WindowsConsoleDriverOptions : ConsoleDriverOptions, IWindowsConsoleDriverOptions
    {
        public bool UseAnsiSequences { get; private set; }
        
        public PaletteOptions PaletteOptions { get; }

        public WindowsConsoleDriverOptions()
        {
            UseAnsiSequences = false;
            IPaletteOptions ioptions = new PaletteOptions();
            ioptions.LoadFromTerminalName("xterm-256color");
            PaletteOptions = (PaletteOptions) ioptions;
        }
        IWindowsConsoleDriverOptions IWindowsConsoleDriverOptions.EnableAnsiSequences()
        {
            UseAnsiSequences = true;
            return this;
        }

    }
}