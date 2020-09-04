using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core;
using Tvision2.Core.Engine;

namespace Tvision2.Blazor.Extensions
{
    public static class WebAssemblyHostBuilder_Tvision2Blazor
    {
        public static WebAssemblyHostBuilder EnableTvision2(this WebAssemblyHostBuilder builder, Action<Tvision2Setup> optionsSetup = null)
        {
            var setup = new Tvision2SetupForBlazor(builder);
            // builder.Properties.Add(Tvision2Options.PropertyKey, setup.Options);
            optionsSetup?.Invoke(setup);
            builder.Services.AddSingleton<Tvision2Options>(setup.Options as Tvision2Options);
            builder.Services.AddScoped<ITuiEngine, TuiEngine>();
            return builder;
        }
    }
}
