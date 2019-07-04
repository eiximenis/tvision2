using System;
using Tvision2.ConsoleDriver.Common;

namespace Tvision2.ConsoleDriver.Common
{
    public class LinuxConsoleDriverOptions : ConsoleDriverOptions, ILinuxConsoleDriverOptions
    {
        public bool UseNCurses { get; private set; }
        public DirectAccessOptions DirectAccessOptions { get; }
        
        public PaletteOptions PaletteOptions { get; }

        public LinuxConsoleDriverOptions()
        {
            UseNCurses = true;
            DirectAccessOptions = new DirectAccessOptions();
            PaletteOptions = new PaletteOptions();
        }

        ILinuxConsoleDriverOptions ILinuxConsoleDriverOptions.UseDirectAccess(Action<IDirectAccessOptions> directAccessOptionsAction = null)
        {
            UseNCurses = false;

            directAccessOptionsAction?.Invoke(DirectAccessOptions);
            return this;
        }

        public ILinuxConsoleDriverOptions UsePalette(Action<IPaletteOptions> paletteOptionsAction = null)
        {
            paletteOptionsAction?.Invoke(PaletteOptions);
            return this;
        }
    }
}