﻿using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Statex;

namespace Tvision2.Core.Engine
{
    public static class StatexTuiEngineBuilderExtensions
    {
        public static TuiEngineBuilder AddStateManager(this TuiEngineBuilder builder, Action<TvStateManager> configAction)
        {
            var manager = new TvStateManager();
            configAction?.Invoke(manager);
            builder.SetCustomItem<TvStateManager>(manager);
            builder.SetCustomItem<ITvStoreSelector>(manager);
            return builder;
        }
    }
}
