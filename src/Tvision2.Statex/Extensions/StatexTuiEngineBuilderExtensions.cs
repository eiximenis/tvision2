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
            setup.ConfigureServices(sc =>
            {
                sc.AddSingleton<TvStateManager>(manager);
                sc.AddSingleton<ITvStoreSelector>(manager);
            });

            setup.Options.AddAfterUpdateAction(() => manager.DoDispatchAllActions());
            return setup;
        }
    }
}
