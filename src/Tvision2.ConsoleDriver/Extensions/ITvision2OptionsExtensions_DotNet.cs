using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using Tvision2.ConsoleDriver;
using Tvision2.ConsoleDriver.Ansi;
using Tvision2.ConsoleDriver.Colors;
using Tvision2.ConsoleDriver.Common;
using Tvision2.Core.Colors;
using Tvision2.Engine.Console;

namespace Tvision2.Core
{

    public static class ITvision2OptionsExtensions_DotNet
    {

        public static Tvision2Setup UseDotNetConsoleDriver(this Tvision2Setup tv2, Action<IConsoleDriverOptions> config = null)
        {
            var options = new ConsoleDriverOptions();
            config?.Invoke(options);
            var colorManager = new DotNetColorManager();
            var driver = new DotNetConsoleDriver(options, colorManager);
            tv2.Options.UseConsoleDriver(driver);
            tv2.ConfigureServices(sc  =>
            {
                sc.AddSingleton<IConsoleDriver>(driver);
                sc.AddSingleton<IColorManager>(colorManager);
            });
            return tv2;
        }
    }
}
