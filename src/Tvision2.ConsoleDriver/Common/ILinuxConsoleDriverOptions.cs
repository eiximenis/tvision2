using System;
using Tvision2.ConsoleDriver.Common;

namespace Tvision2.ConsoleDriver.Common
{
    public interface ILinuxConsoleDriverOptions : IConsoleDriverOptions
    {
        ILinuxConsoleDriverOptions UseDirectAccess(Action<IDirectAccessOptions> directAccessOptionsAction = null);
        ILinuxConsoleDriverOptions UsePalette(Action<IPaletteOptions> paletteOptionsAction = null);
    }
}