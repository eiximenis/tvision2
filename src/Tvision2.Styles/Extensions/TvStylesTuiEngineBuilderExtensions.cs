using Microsoft.Extensions.DependencyInjection;
using System;
using Tvision2.Core;
using Tvision2.Core.Colors;
using Tvision2.Core.Engine;
using Tvision2.Styles;

namespace Tvision2.DependencyInjection
{
    public static class TvStylesTuiEngineBuilderExtensions
    {
        private const string stylesSetupStep = "TvStyles";
        public static Tvision2Setup AddStyles(this Tvision2Setup setup, Action<ISkinManagerBuilder> skinOptions = null)
        {
            setup.ConfigureServices(sc => sc.AddScoped<ISkinManager, SkinManager>());
            setup.ConfigureServices(sc => sc.AddScoped<StyledRenderContextAdatper>());
            setup.AddSetupStep(stylesSetupStep);

            setup.Options.AfterCreateInvoke((engine, sp) =>
            {
                var skinManager = sp.GetRequiredService<ISkinManager>() as SkinManager;
                var colorManager = sp.GetRequiredService<IColorManager>();
                if (skinOptions != null)
                {
                    FillSkinManager(skinOptions, skinManager, colorManager);
                }
                else
                {
                    FillDefaultSkinManager(skinManager, colorManager);
                }

                var styledAdapter = sp.GetRequiredService<StyledRenderContextAdatper>();
                styledAdapter.AttachTo(engine.UI as ComponentTree);
            });

            return setup;
        }


        public static bool HasStylesEnabled(this Tvision2Setup setup)
        {
            return setup.HasSetupStep(stylesSetupStep);
        }

        private static void FillDefaultSkinManager(SkinManager sm, IColorManager cm)
        {
            // TODO: Create a default skin manager.

            var skinBuilder = new SkinManagerBuilder(cm);
            skinBuilder.AddSkin(string.Empty);
            skinBuilder.Fill(sm);
        }

        private static void FillSkinManager(Action<ISkinManagerBuilder> builderOptions, SkinManager sm, IColorManager cm)
        {
            var skinManagerBuilder = new SkinManagerBuilder(cm);
            builderOptions.Invoke(skinManagerBuilder);
            skinManagerBuilder.Fill(sm);
        }
    }
}
