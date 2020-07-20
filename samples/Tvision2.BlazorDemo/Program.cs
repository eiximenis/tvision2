using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Tvision2.Blazor.Extensions;
using Microsoft.Extensions.Hosting;
using Tvision2.Core;
using Tvision2.Core.Engine;
using Tvision2.DependencyInjection;
using Tvision2.Controls.Styles;

namespace Tvision2.BlazorDemo
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.EnableTvision2(s => 
                    s.UseBlazor(opt => opt.WithPalette(p => p.UseBasicColorMode()))
                    .UseViewportManager()
                    .UseLayoutManager()
                    .AddTvision2Startup<Startup>()
                    .AddTvControls(sk => sk.AddMcStyles())
                .AddTvision2Startup<Startup>());;
            builder.RootComponents.Add<App>("app");
            builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            await builder.Build().RunAsync();
        }
    }
}
