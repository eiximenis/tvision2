using System;
using Tvision2.ConsoleDriver.Common;

namespace Tvision2.ConsoleDriver.Win32
{
    public interface IWindowsConsoleDriverOptions : IConsoleDriverOptions
    {
        IWindowsAnsiDriverOptions UseAnsi();
    }

    public interface IWindowsAnsiDriverOptions
    {
        IWindowsConsoleDriverOptions WithPalette(Action<IPaletteOptions> paletteOptionsAction = null);
        IWindowsAnsiDriverOptions EnableTrueColor();
    }
}