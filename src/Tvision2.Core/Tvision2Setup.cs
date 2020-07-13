using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Runtime.CompilerServices;
using Tvision2.Core.Hooks;

namespace Tvision2.Core
{
    public abstract class Tvision2Setup
    {
        private readonly Tvision2Options _options;
        public ITvision2Options Options => _options;

        
        public Tvision2Setup()
        {
            _options = new Tvision2Options();
        }

        public abstract void ConfigureServices(Action<IServiceCollection> configureDelegate);

        public Tvision2Setup AddHook<THook>()
            where THook : class, IEventHook
        {

            ConfigureServices(sc =>
            {
                sc.AddTransient<THook>();
            });
            _options.AddHook<THook>();

            return this;

        }

    }

}
