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


        public static Tvision2Setup AddTvControls(this Tvision2Setup setup, Action<IControlsOptions> controlsOptionsAction = null)
        {

            var options = new TvControlsOptions();

            controlsOptionsAction?.Invoke(options);
            
            if (!setup.HasStylesEnabled())
            {
                setup.AddStyles(options.SkinOptions);
            }

            setup.ConfigureServices(sc =>
            {
                sc.AddScoped<IControlsTree, ControlsTree>();

            });

            if (options.MouseManagerEnabled)
            {
                setup.AddHook<MouseChangeFocusEventHook>();
            }
            
            setup.AddHook<ChangeFocusEventHook>();

            return setup;
            
        }
    }

    public interface IControlsOptions
    {
        IControlsOptions ConfigureSkins(Action<ISkinManagerBuilder> options);
        IControlsOptions EnableMouseManager();
    }
}
