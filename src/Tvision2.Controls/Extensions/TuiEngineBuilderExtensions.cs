using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Tvision2.Controls;
using Tvision2.Controls.Hooks;
using Tvision2.Controls.Styles;
using Tvision2.Core;
using Tvision2.Core.Colors;

namespace Tvision2.DependencyInjection
{
    public static class TvControlsTuiEngineBuilderExtensions
    {
        public static Tvision2Setup AddTvControls(this Tvision2Setup setup, Action<ISkinManagerBuilder> skinOptions = null)
        {

            var tree = new ControlsTree();

            setup.Builder.ConfigureServices(sc =>
            {
                sc.AddSingleton<ControlsTree>(tree);
                sc.AddSingleton<IControlsTree>(tree);
                sc.AddSingleton<ISkinManager>(new SkinManager());
            });

            setup.AddHook<ChangeFocusEventHook>();

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


                var ctree = sp.GetRequiredService<IControlsTree>() as ControlsTree;
                tree.AttachTo(engine.UI);
            });

            return setup;
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
