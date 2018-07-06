using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Tvision2.Core;
using Tvision2.Statex;

namespace Tvision2.DependencyInjection
{
    public static class StatexTuiEngineBuilderExtensions
    {
        public static Tvision2Setup AddStateManager(this Tvision2Setup setup, Action<TvStateManager> configAction)
        {
            var manager = new TvStateManager();
            configAction?.Invoke(manager);
            setup.Builder.ConfigureServices(sc =>
            {
                sc.AddSingleton<TvStateManager>(manager);
                sc.AddSingleton<ITvStoreSelector>(manager);

            });
            return setup;
        }
    }
}
