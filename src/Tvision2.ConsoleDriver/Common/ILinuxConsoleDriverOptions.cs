using System;
using Tvision2.ConsoleDriver.Common;

namespace Tvision2.ConsoleDriver.Common
{
    public interface ILinuxConsoleDriverOptions : IConsoleDriverOptions
    {
        ILinuxAnsiDriverOptions UseAnsi();
        INCursesDriverOptions UseNCurses();
    }

    
    
    public interface INCursesDriverOptions
    {
        INCursesDriverOptions WithPalette(Action<IPaletteOptions> paletteOptionsAction = null);
    }

    public interface ILinuxAnsiDriverOptions
    {
        ILinuxAnsiDriverOptions WithPalette(Action<IPaletteOptions> paletteOptionsAction = null);
        ILinuxAnsiDriverOptions EnableTrueColor(Action<ITrueColorOptions> truecolorOptionsAction = null);
    }
    

}