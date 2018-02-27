using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Hooks;
using Tvision2.Core.Styles;

namespace Tvision2.Core.Engine
{
    public static class CoreTuiEngineBuilderExtensions
    {

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
