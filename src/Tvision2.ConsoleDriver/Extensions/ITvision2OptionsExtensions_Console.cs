using Microsoft.Extensions.DependencyInjection;
using System;
using Tvision2.ConsoleDriver;
using Tvision2.Engine.Console;

namespace Tvision2.Core
{

    public static class ITvision2OptionsExtensions_Console
    {
        public static Tvision2Setup UseWin32ConsoleDriver(this Tvision2Setup tv2, Action<IConsoleDriverOptions> config = null)
        {
            var options = new ConsoleDriverOptions();
            config?.Invoke(options);
            var driver = new WinConsoleDriver(options);
            tv2.Options.UseConsoleDriver(driver);
            tv2.Builder.ConfigureServices((hc, sc) => 
            {
                sc.AddSingleton<IConsoleDriver>(driver);
            });
            return tv2;
        }

        public static Tvision2Setup UseNcursesConsoleDriver(this Tvision2Setup tv2, Action<IConsoleDriverOptions> config = null)
        {
            var options = new ConsoleDriverOptions();
            config?.Invoke(options);
            var driver = new WinConsoleDriver(options);
            tv2.Options.UseConsoleDriver(driver);
            tv2.Builder.ConfigureServices((hc, sc) =>
            {
                sc.AddSingleton<IConsoleDriver>(driver);
            });
            return tv2;
        }

        public static Tvision2Setup UseDotNetConsoleDriver(this Tvision2Setup tv2, Action<IConsoleDriverOptions> config = null)
        {
            var options = new ConsoleDriverOptions();
            config?.Invoke(options);
            var driver = new NetConsoleDriver(options);
            tv2.Options.UseConsoleDriver(driver);
            tv2.Builder.ConfigureServices((hc, sc) =>
            {
                sc.AddSingleton<IConsoleDriver>(driver);
            });
            return tv2;
        }

        public static Tvision2Setup UsePlatformConsoleDriver(this Tvision2Setup tv2, Action<IConsoleDriverOptions> config = null)
        {
            var options = new ConsoleDriverOptions();
            config?.Invoke(options);
            var platform = Environment.OSVersion.Platform;
            var useWin = (platform == PlatformID.Win32NT || platform == PlatformID.Win32S || platform == PlatformID.Win32Windows);
            var driver = useWin ? new WinConsoleDriver(options) as IConsoleDriver : new TerminfoConsoleDriver() as IConsoleDriver;
            tv2.Options.UseConsoleDriver(driver);
            tv2.Builder.ConfigureServices((hc, sc) =>
            {
                sc.AddSingleton<IConsoleDriver>(driver);
            });
            return tv2;
        }
    }
}
