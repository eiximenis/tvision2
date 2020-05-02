using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using Tvision2.ConsoleDriver;
using Tvision2.ConsoleDriver.Ansi;
using Tvision2.ConsoleDriver.Colors;
using Tvision2.ConsoleDriver.Common;
using Tvision2.ConsoleDriver.Win32;
using Tvision2.Core.Colors;
using Tvision2.Engine.Console;

namespace Tvision2.Core
{

    public static class ITvision2OptionsExtensions_Console
    {
        public static Tvision2Setup UseWin32ConsoleDriver(this Tvision2Setup tv2, Action<IWindowsConsoleDriverOptions> config = null)
        {
            var options = new WindowsConsoleDriverOptions();
            config?.Invoke(options);
            return tv2.UseWin32ConsoleDriver(options);
        }

        private static Tvision2Setup UseWin32ConsoleDriver(this Tvision2Setup tv2, WindowsConsoleDriverOptions options)
        {

            var driverFactory = new Win32ConsoleDriverFactory(options);
            var driver = driverFactory.CreateWindowsDriver();


            tv2.Options.UseConsoleDriver(driver);
            tv2.Builder.ConfigureServices((hc, sc) =>
            {
                sc.AddSingleton<IConsoleDriver>(driver);
                sc.AddSingleton<IColorManager>(driver.ColorManager);
            });
            return tv2;
        }
        

        public static Tvision2Setup UseNcursesConsoleDriver(this Tvision2Setup tv2, Action<IConsoleDriverOptions> config = null)
        {
            var options = new LinuxConsoleDriverOptions();
            config?.Invoke(options);
            return tv2.UseNcursesConsoleDriver(options);
        }

        private static Tvision2Setup UseNcursesConsoleDriver(this Tvision2Setup tv2, LinuxConsoleDriverOptions options)
        {
            var colorManager = new NcursesColorManager(options.PaletteOptions);
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
        
        public static Tvision2Setup UseAnsiLinuxConsoleDriver(this Tvision2Setup tv2, LinuxConsoleDriverOptions options)
        {
            var colorManager = new AnsiColorManager(options.PaletteOptions);
            var driver = new AnsiLinuxConsoleDriver(options, colorManager);
            tv2.Options.UseConsoleDriver(driver);
            tv2.Builder.ConfigureServices((hc, sc) =>
            {
                sc.AddSingleton<IConsoleDriver>(driver);
                sc.AddSingleton<IColorManager>(colorManager);
            });
            return tv2;
        }
        
        

        public static Tvision2Setup UsePlatformConsoleDriver(this Tvision2Setup tv2, Action<IPlatformConsoleDriverOptions> config = null)
        {
            var platformOptions = new PlatformConsoleDriverOptions();
            config?.Invoke(platformOptions);
            var platform = Environment.OSVersion.Platform;
            var useWin = (platform == PlatformID.Win32NT || platform == PlatformID.Win32S || platform == PlatformID.Win32Windows);
            if (useWin)
            {
                return UseWin32ConsoleDriver(tv2, platformOptions.WindowsOptions);
            }

            if (platformOptions.LinuxOptions.UseNCurses)
            {
                return UseNcursesConsoleDriver(tv2, platformOptions.LinuxOptions);
            }

            return UseAnsiLinuxConsoleDriver(tv2, platformOptions.LinuxOptions);
        }
    }
}
