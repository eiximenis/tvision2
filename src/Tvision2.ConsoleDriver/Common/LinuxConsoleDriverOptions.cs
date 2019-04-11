using System;

namespace Tvision2.ConsoleDriver.Common
{
    public class LinuxConsoleDriverOptions : ConsoleDriverOptions, ILinuxConsoleDriverOptions
    {
        public bool UseNCurses { get; private set; }
        
        public DirectAccessOptions DirectAccessOptions { get; }

        public LinuxConsoleDriverOptions()
        {
            UseNCurses = true;
            DirectAccessOptions = new DirectAccessOptions();
        }

        ILinuxConsoleDriverOptions ILinuxConsoleDriverOptions.UseDirectAccess(Action<IDirectAccessOptions> directAccessOptions = null)
        {
            UseNCurses = false;

            directAccessOptions?.Invoke(DirectAccessOptions);
            return this;
        }
        
    }
}