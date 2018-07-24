using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Tvision2.Viewports;

namespace Tvision2.Core.Engine
{
    public static class TuiEngineBuilderExtensions_Viewports
    {

        public static Tvision2Setup UseViewportManager(this Tvision2Setup setup)
        {

            setup.Builder.ConfigureServices(sc =>
            {
                sc.AddSingleton<IViewportManager, ViewportManager>();
            });
            

            setup.Options.AfterCreateInvoke((engine, sp) =>
            {
                var viewportManager = sp.GetRequiredService<IViewportManager>() as ViewportManager;
                viewportManager.AttachTo(engine.UI as ComponentTree);
            });
            return setup;
        }
    }
}
