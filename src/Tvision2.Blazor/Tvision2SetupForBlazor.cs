using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core;

namespace Tvision2.Blazor
{
    public class Tvision2SetupForBlazor : Tvision2Setup
    {
        private readonly WebAssemblyHostBuilder _builder;
        public Tvision2SetupForBlazor(WebAssemblyHostBuilder builder)
        {
            _builder = builder;
        }

        public override void ConfigureServices(Action<IServiceCollection> configureDelegate)
        {
            configureDelegate(_builder.Services);
        }

    }
}
