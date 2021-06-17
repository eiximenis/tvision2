using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Tvision2.Engine.Console;
using Tvision2.Viewports;
using Tvision2.Viewports.Hooks;

namespace Tvision2.Core.Engine
{
    public static class TuiEngineBuilderExtensions_Viewports
    {

        public static Tvision2Setup UseViewportManager(this Tvision2Setup setup, Action<IViewportManagerOptions> optionsConfigurer = null)
        {

            var options = new ViewportManagerOptions();
            optionsConfigurer?.Invoke(options);

            if (options.DynamicViewportsEnabled)
            {

                setup.AddHook<WindowResizedHook>();
                setup.ConfigureServices(sc =>
                {
                    sc.AddTransient<IViewportFactory, DynamicViewportFactory>();
                    sc.AddTransient<IDynamicViewportFactory, DynamicViewportFactory>();
                    sc.AddScoped<IViewportManager, ViewportManager>();
                });
            }
            else
            {
                setup.ConfigureServices(sc =>
                {
                    sc.AddTransient<IViewportFactory, ViewportFactory>();
                    sc.AddScoped<IViewportManager, ViewportManager>();
                });
            }

            setup.Options.AfterCreateInvoke((engine, sp) =>
            {
                var viewportManager = sp.GetRequiredService<IViewportManager>() as ViewportManager;
                viewportManager.AttachTo(engine.UI as ComponentTree);
            });
            return setup;
        }
    }
}
