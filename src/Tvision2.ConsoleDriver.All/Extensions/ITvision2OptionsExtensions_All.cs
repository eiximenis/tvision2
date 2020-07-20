using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.ConsoleDriver.Common;

namespace Tvision2.Core
{
    public static class ITvision2OptionsExtensions_All
    {
        public static Tvision2Setup UsePlatformConsoleDriver(this Tvision2Setup tv2, Action<IPlatformConsoleDriverOptions> config = null)
        {
            var platformOptions = new PlatformConsoleDriverOptions();
            config?.Invoke(platformOptions);
            var platform = Environment.OSVersion.Platform;
            var useWin = (platform == PlatformID.Win32NT || platform == PlatformID.Win32S || platform == PlatformID.Win32Windows);
            if (useWin)
            {
                return tv2.UseWin32ConsoleDriver(platformOptions.WindowsOptions);
            }

            if (platformOptions.LinuxOptions.UseNCurses)
            {
                return tv2.UseNcursesConsoleDriver(platformOptions.LinuxOptions);
            }

            return  tv2.UseAnsiLinuxConsoleDriver(platformOptions.LinuxOptions);
        }
    }
}
