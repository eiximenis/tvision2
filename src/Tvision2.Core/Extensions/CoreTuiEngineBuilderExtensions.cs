using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Hooks;

namespace Tvision2.Core.Engine
{
    public static class CoreTuiEngineBuilderExtensions
    {
        public static TuiEngineBuilder AddKeyboardHook(this TuiEngineBuilder builder)
        {
            builder.SetCustomItem("__KeyboardHook", new KeyboardHook());
            return builder;
        }
    }
}
