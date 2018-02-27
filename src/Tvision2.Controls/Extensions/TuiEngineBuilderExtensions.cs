using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Controls;
using Tvision2.Controls.Hooks;

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
            return builder;
        }

    }
}
