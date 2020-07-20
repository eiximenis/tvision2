using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.ConsoleDriver;
using Tvision2.ConsoleDriver.Ansi;
using Tvision2.ConsoleDriver.Common;
using Tvision2.Core.Colors;
using Tvision2.Engine.Console;

namespace Tvision2.Core
{
    public static class ITvision2OptionsExtensions_Linux
    {

        public static Tvision2Setup UseNcursesConsoleDriver(this Tvision2Setup tv2, Action<IConsoleDriverOptions> config = null)
        {
            var options = new LinuxConsoleDriverOptions();
            config?.Invoke(options);
            return tv2.UseNcursesConsoleDriver(options);
        }

        public static Tvision2Setup UseNcursesConsoleDriver(this Tvision2Setup tv2, LinuxConsoleDriverOptions options)
        {
            var colorManager = new NcursesColorManager(options.PaletteOptions);
            var driver = new NcursesConsoleDriver(options, colorManager);
            tv2.Options.UseConsoleDriver(driver);
            tv2.ConfigureServices(sc =>
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
            tv2.ConfigureServices(sc =>
            {
                sc.AddSingleton<IConsoleDriver>(driver);
                sc.AddSingleton<IColorManager>(colorManager);
            });
            return tv2;
        }
    }
}
