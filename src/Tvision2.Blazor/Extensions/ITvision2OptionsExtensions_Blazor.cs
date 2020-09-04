using Microsoft.Extensions.DependencyInjection;
using System;
using Tvision2.ConsoleDriver;
using Tvision2.ConsoleDriver.Ansi;
using Tvision2.ConsoleDriver.Blazor;
using Tvision2.Core.Colors;
using Tvision2.Engine.Console;

namespace Tvision2.Core
{

    public static class ITvision2OptionsExtensions_Blazor
    {
        public static Tvision2Setup UseBlazor(this Tvision2Setup tv2, Action<IBlazorConsoleDriverOptions> config = null)
        {

            var options = new BlazorConsoleDriverOptions();
            config?.Invoke(options);
            var colorManager = new AnsiColorManager(options.PaletteOptions);
            var driver = new BtermConsoleDriver(colorManager);
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
