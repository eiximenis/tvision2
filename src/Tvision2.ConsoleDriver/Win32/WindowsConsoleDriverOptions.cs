using System;
using Tvision2.ConsoleDriver.Common;

namespace Tvision2.ConsoleDriver.Win32
{
    public class WindowsConsoleDriverOptions : ConsoleDriverOptions,
        IWindowsConsoleDriverOptions, IWindowsAnsiDriverOptions
    {
        public bool UseAnsiSequences { get; private set; }

        public PaletteOptions PaletteOptions { get; }

        public WindowsConsoleDriverOptions()
        {
            UseAnsiSequences = false;
            IPaletteOptions ioptions = new PaletteOptions();
            ioptions.LoadFromTerminalName("xterm-256color");
            PaletteOptions = (PaletteOptions)ioptions;
        }
        IWindowsAnsiDriverOptions IWindowsConsoleDriverOptions.UseAnsi()
        {
            UseAnsiSequences = true;
            return this;
        }

        IWindowsConsoleDriverOptions IWindowsAnsiDriverOptions.WithPalette(Action<IPaletteOptions> paletteOptionsAction = null)
        {
            paletteOptionsAction?.Invoke(PaletteOptions);
            return this;
        }

    }
}