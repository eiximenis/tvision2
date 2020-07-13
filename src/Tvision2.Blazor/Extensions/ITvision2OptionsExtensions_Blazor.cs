using Microsoft.Extensions.DependencyInjection;
using System;
using Tvision2.ConsoleDriver.BlazorTerm;
using Tvision2.Core.Colors;
using Tvision2.Engine.Console;

namespace Tvision2.Core
{

    public static class ITvision2OptionsExtensions_Blazor
    {
        public static Tvision2Setup UseBlazor(this Tvision2Setup tv2)
        {
            var driver = new BtermConsoleDriver();
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
