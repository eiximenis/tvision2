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
        public static Tvision2Setup UseDebug(this Tvision2Setup setup, Action<ITvision2DebugOptions>? optionsSetup = null)
        {
            setup.ConfigureServices(sc =>
            {
                var options = new Tvision2DebugOptions();
                optionsSetup?.Invoke(options);
                var debugger = new Tvision2Debugger(options);
                sc.AddSingleton<ITvision2Debugger>(debugger);
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
