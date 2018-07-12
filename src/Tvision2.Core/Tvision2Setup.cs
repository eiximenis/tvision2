using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Tvision2.Core.Hooks;

namespace Tvision2.Core
{
    public class Tvision2Setup 
    {
        private readonly Tvision2Options _options;
        public ITvision2Options Options => _options;

        public IHostBuilder Builder { get; }
        public Tvision2Setup(IHostBuilder builder)
        {
            Builder = builder ?? throw new ArgumentNullException(nameof(builder));
            _options = new Tvision2Options();
        }

        public Tvision2Setup AddHook<THook>()
            where THook : class, IEventHook
        {
            Builder.ConfigureServices(sc =>
            {
                sc.AddTransient<THook>();
            });
            _options.AddHook<THook>();

            return this;

        }

    }
}
