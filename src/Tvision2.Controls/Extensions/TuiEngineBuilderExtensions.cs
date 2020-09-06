using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Tvision2.Controls;
using Tvision2.Controls.Hooks;
using Tvision2.Core;
using Tvision2.Core.Colors;
using Tvision2.Styles;

namespace Tvision2.DependencyInjection
{
    public static class TvControlsTuiEngineBuilderExtensions
    {

        public static Tvision2Setup AddTvControls(this Tvision2Setup setup, Action<ISkinManagerBuilder> skinOptions = null)
        {

            if (!setup.HasStylesEnabled())
            {
                setup.AddStyles(skinOptions);
            }

            setup.ConfigureServices(sc =>
            {
                sc.AddScoped<IControlsTree, ControlsTree>();

            });

            setup.AddHook<ChangeFocusEventHook>();

            return setup;
        }
    }
}
