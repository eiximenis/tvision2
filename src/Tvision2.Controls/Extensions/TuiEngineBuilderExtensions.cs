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
                var sp = sc.BuildServiceProvider();
                var cm = sp.GetService<IColorManager>();
                if (skinOptions != null)
                {
                    var skinManager = CreateSkinManager(skinOptions, cm);
                    sc.AddSingleton<ISkinManager>(skinManager);
                }
                else
                {
                    var skinManager = CreateDefaultSkinManager(cm);
                    sc.AddSingleton<ISkinManager>(skinManager);
                }
            });

            setup.AddHook<ChangeFocusEventHook>();
            setup.Options.AfterCreateInvoke((engine, sp) =>
            {
                var ctree = sp.GetRequiredService<IControlsTree>() as ControlsTree;
                tree.AttachTo(engine.UI);
            });

            return setup;
        }

        private static ISkinManager CreateDefaultSkinManager(IColorManager cm)
        {
            // TODO: Create a default skin manager.

            var skinBuilder = new SkinManagerBuilder(cm);
            skinBuilder.AddSkin(string.Empty);
            var skinManager = skinBuilder.Build();
            return skinManager;
        }

        private static ISkinManager CreateSkinManager(Action<ISkinManagerBuilder> builderOptions, IColorManager cm)
        {
            var skinManagerBuilder = new SkinManagerBuilder(cm);
            builderOptions.Invoke(skinManagerBuilder);
            var skinManager = skinManagerBuilder.Build();

            return skinManager;
        }

    }
}
