using Microsoft.Extensions.DependencyInjection;
using System;
using Tvision2.ConsoleDriver;
using Tvision2.ConsoleDriver.Colors;
using Tvision2.ConsoleDriver.NCurses;
using Tvision2.ConsoleDriver.Win32;
using Tvision2.Core.Colors;
using Tvision2.Engine.Console;

namespace Tvision2.Core
{

    public static class ITvision2OptionsExtensions_Console
    {
        public static Tvision2Setup UseWin32ConsoleDriver(this Tvision2Setup tv2, Action<IConsoleDriverOptions> config = null)
        {
            var options = new ConsoleDriverOptions();
            config?.Invoke(options);
            var driver = new Win32ConsoleDriver(options);
            var colorManager = driver.SupportsVt100
                // TODO: Change for using Win32Vt100ColorManager if allowed
                ? (IWindowsColorManager)new Win32StdColorManager()
                : (IWindowsColorManager)new Win32StdColorManager();
            driver.AttachColorManager(colorManager);
            tv2.Options.UseConsoleDriver(driver);
            tv2.Builder.ConfigureServices((hc, sc) =>
            {
                sc.AddSingleton<IConsoleDriver>(driver);
                sc.AddSingleton<IColorManager>(colorManager);
            });
            return tv2;
        }

        public static Tvision2Setup UseNcursesConsoleDriver(this Tvision2Setup tv2, Action<IConsoleDriverOptions> config = null)
        {
            var options = new ConsoleDriverOptions();
            config?.Invoke(options);
            var colorManager = new NcursesColorManager();
            var driver = new NcursesConsoleDriver(options, colorManager);
            tv2.Options.UseConsoleDriver(driver);
            tv2.Builder.ConfigureServices((hc, sc) =>
            {
                sc.AddSingleton<IConsoleDriver>(driver);
                sc.AddSingleton<IColorManager>(colorManager);
            });
            return tv2;
        }

        public static Tvision2Setup UseDotNetConsoleDriver(this Tvision2Setup tv2, Action<IConsoleDriverOptions> config = null)
        {
            var options = new ConsoleDriverOptions();
            config?.Invoke(options);
            var colorManager = new DotNetColorManager();
            var driver = new DotNetConsoleDriver(options, colorManager);
            tv2.Options.UseConsoleDriver(driver);
            tv2.Builder.ConfigureServices((hc, sc) =>
            {
                sc.AddSingleton<IConsoleDriver>(driver);
                sc.AddSingleton<IColorManager>(colorManager);
            });
            return tv2;
        }

        public static Tvision2Setup UsePlatformConsoleDriver(this Tvision2Setup tv2, Action<IConsoleDriverOptions> config = null)
        {
            var platform = Environment.OSVersion.Platform;
            var useWin = (platform == PlatformID.Win32NT || platform == PlatformID.Win32S || platform == PlatformID.Win32Windows);
            return useWin ? UseWin32ConsoleDriver(tv2, config) : UseNcursesConsoleDriver(tv2, config);
        }
    }
}
