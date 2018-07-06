using System;
using Tvision2.ConsoleDriver;
using Tvision2.Engine.Console;

namespace Tvision2.Core
{

    public static class ITvision2OptionsExtensions_Console
    {
        public static Tvision2Setup UseWin32ConsoleDriver(this Tvision2Setup tv2, Action<ConsoleDriverOptions> config = null)
        {
            var options = new ConsoleDriverOptions();
            config?.Invoke(options);
            tv2.Options.UseConsoleDriver(new WinConsoleDriver(options));
            return tv2;
        }

        public static Tvision2Setup UseNcursesConsoleDriver(this Tvision2Setup tv2, Action<ConsoleDriverOptions> config = null)
        {
            var options = new ConsoleDriverOptions();
            config?.Invoke(options);
            tv2.Options.UseConsoleDriver(new NcursesConsoleDriver(options));
            return tv2;
        }

        public static Tvision2Setup UseDotNetConsoleDriver(this Tvision2Setup tv2, Action<ConsoleDriverOptions> config = null)
        {
            var options = new ConsoleDriverOptions();
            config?.Invoke(options);
            tv2.Options.UseConsoleDriver(new NetConsoleDriver(options));
            return tv2;
        }

        public static Tvision2Setup UsePlatformConsoleDriver(this Tvision2Setup tv2, Action<ConsoleDriverOptions> config = null)
        {
            var options = new ConsoleDriverOptions();
            config?.Invoke(options);
            var platform = Environment.OSVersion.Platform;
            var useWin = (platform == PlatformID.Win32NT || platform == PlatformID.Win32S || platform == PlatformID.Win32Windows);
            var driver = useWin ? new WinConsoleDriver(options) as IConsoleDriver : new NcursesConsoleDriver(options) as IConsoleDriver;
            tv2.Options.UseConsoleDriver(driver);
            return tv2;
        }
    }
}
