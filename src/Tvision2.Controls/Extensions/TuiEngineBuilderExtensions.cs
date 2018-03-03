using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Controls;
using Tvision2.Controls.Hooks;
using Tvision2.Controls.Styles;

namespace Tvision2.Core.Engine
{
    public static class TvControlsTuiEngineBuilderExtensions
    {
        public static TuiEngineBuilder AddTvControls(this TuiEngineBuilder builder)
        {
            var tree = new ControlsTree();
            builder.SetCustomItem<ControlsTree>(tree);
            builder.SetCustomItem<IControlsTree>(tree);
            builder.AddEventHook(new ChangeFocusEventHook());
            builder.BeforeCreateInvoke(b =>
            {
                if (b.GetCustomItem<ISkinManager>() == null)
                {
                    AddDefaultSkinManager(b);
                }
            });
            builder.AfterCreateInvoke(engine => tree.AttachToComponentTree(engine.UI));
            return builder;
        }

        private static void AddDefaultSkinManager(TuiEngineBuilder builder)
        {
            // TODO: Create a default skin manager.
            var skinBuilder = new SkinManagerBuilder();
            skinBuilder.AddSkin(string.Empty);
            var skinManager = skinBuilder.Build();
            builder.SetCustomItem<ISkinManager>(skinManager);
        }

        public static TuiEngineBuilder AddSkinSupport(this TuiEngineBuilder builder, Action<ISkinManagerBuilder> builderOptions)
        {
            var skinManagerBuilder = new SkinManagerBuilder();
            builderOptions.Invoke(skinManagerBuilder);
            var skinManager = skinManagerBuilder.Build();
            builder.SetCustomItem<ISkinManager>(skinManager);
            return builder;
        }


    }
}
