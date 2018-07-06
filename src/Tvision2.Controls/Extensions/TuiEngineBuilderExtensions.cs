using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Tvision2.Controls;
using Tvision2.Controls.Hooks;
using Tvision2.Controls.Styles;
using Tvision2.Core;

namespace Tvision2.DependencyInjection
{
    public static class TvControlsTuiEngineBuilderExtensions
    {
        public static Tvision2Setup AddTvControls(this Tvision2Setup setup)
        {

            var tree = new ControlsTree();

            setup.Builder.ConfigureServices(sc =>
            {
                sc.AddSingleton<ControlsTree>(tree);
                sc.AddSingleton<IControlsTree>(tree);
                var sp = sc.BuildServiceProvider();
                var skinManager = sp.GetService<ISkinManager>();
                if (skinManager == null)
                {
                    AddDefaultSkinManager(sc);
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

        private static void AddDefaultSkinManager(IServiceCollection sc)
        {
            // TODO: Create a default skin manager.
            var skinBuilder = new SkinManagerBuilder();
            skinBuilder.AddSkin(string.Empty);
            var skinManager = skinBuilder.Build();
            sc.AddSingleton<ISkinManager>(skinManager);
        }

        public static Tvision2Setup AddSkinSupport(this Tvision2Setup setup, Action<ISkinManagerBuilder> builderOptions)
        {
            var skinManagerBuilder = new SkinManagerBuilder();
            builderOptions.Invoke(skinManagerBuilder);
            var skinManager = skinManagerBuilder.Build();
            setup.Builder.ConfigureServices(sc =>
            {
               sc.AddSingleton<ISkinManager>(skinManager);
            });
            return setup;
        }

    }
}
