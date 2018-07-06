using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Tvision2.Core;
using Tvision2.Core.Engine;

namespace Microsoft.Extensions.Hosting
{
    public static class IHostBuilderExtensions_Tvison2
    {
        public static IHostBuilder UseTvision2(this IHostBuilder builder, Action<Tvision2Setup> optionsSetup = null)
        {
            var setup = new Tvision2Setup(builder);
            builder.Properties.Add(Tvision2Options.PropertyKey, setup.Options);
            optionsSetup?.Invoke(setup);
            builder.ConfigureServices(sc =>
            {
                sc.Configure<ConsoleLifetimeOptions>(o => o.SuppressStatusMessages = true);
            });

            builder.ConfigureServices((hc, sc) =>
            {
                sc.AddSingleton<Tvision2Options>(setup.Options as Tvision2Options);
                sc.AddSingleton<ITuiEngine, TuiEngine>();
                sc.AddHostedService<TuiEngineHost>();
            });

            return builder;
        }

        public static Tvision2Setup AddTvision2Startup<TStartup>(this Tvision2Setup tv2setup) where TStartup : class, ITvisionAppStartup
        {
            tv2setup.Builder.ConfigureServices(sc =>
            {
                sc.AddTransient<ITvisionAppStartup, TStartup>();
            });

            return tv2setup;
        }

        public static Task RunTvisionConsoleApp(this IHostBuilder builder, CancellationToken cancellationToken = default(CancellationToken))
        {
            var tv2options = builder.Properties.TryGetValue(Tvision2Options.PropertyKey, out object options) ? options as Tvision2Options : null;
            if (tv2options == null)
            {
                throw new ArgumentException("Must add Tvision2 first. Please call UseTvision2 method before.");
            }

            builder.ConfigureServices(sc =>
            {
                sc.AddHostedService<TuiEngineHost>();
            });
            return builder.RunConsoleAsync(cancellationToken);
        }
    }
}
