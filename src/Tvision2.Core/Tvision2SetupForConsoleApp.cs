using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Tvision2.Core
{
    public sealed class Tvision2SetupForConsoleApp  : Tvision2Setup
    {

        private readonly IHostBuilder _builder;
        public Tvision2SetupForConsoleApp(IHostBuilder builder)
        {
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
        }
        public override void ConfigureServices(Action<IServiceCollection> configureDelegate)
        {
            _builder.ConfigureServices(configureDelegate);
        }

    }

}
