using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.ConsoleDriver;
using Tvision2.ConsoleDriver.Ansi;
using Tvision2.ConsoleDriver.Common;
using Tvision2.ConsoleDriver.Win32;
using Tvision2.Core.Colors;
using Tvision2.Engine.Console;

namespace Tvision2.Core
{
    public static class ITvision2OptionsExtensions_Windows
    {
        public static Tvision2Setup UseWin32ConsoleDriver(this Tvision2Setup tv2, Action<IWindowsConsoleDriverOptions> config = null)
        {
            var options = new WindowsConsoleDriverOptions();
            config?.Invoke(options);
            return tv2.UseWin32ConsoleDriver(options);
        }

        public static Tvision2Setup UseWin32ConsoleDriver(this Tvision2Setup tv2, WindowsConsoleDriverOptions options)
        {

            var driverFactory = new Win32ConsoleDriverFactory(options);
            var driver = driverFactory.CreateWindowsDriver();
            tv2.Options.UseConsoleDriver(driver);
            tv2.ConfigureServices(sc =>
            {
                sc.AddSingleton<IConsoleDriver>(driver);
                sc.AddSingleton<IColorManager>(driver.ColorManager);
            });
            return tv2;
        }
    }
}
