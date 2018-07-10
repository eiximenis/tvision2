using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core;
using Tvision2.Debug;

namespace Tvision2.Core.Engine
{
    public static class Tvision2SetupExtensions_Debug
    {
        public static Tvision2Setup UseDebug(this Tvision2Setup setup)
        {
            setup.Builder.ConfigureServices((hc, sc) =>
            {
                sc.AddSingleton<ITvision2Debugger, Tvision2Debugger>();
            });

            setup.Options.AfterCreateInvoke((engine, sp) =>
            {
                var viewportManager = sp.GetRequiredService<ITvision2Debugger>();
                viewportManager.AttachTo(engine.UI);
            });
            return setup;
        }
    }
}
