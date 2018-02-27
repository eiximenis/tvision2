using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Statex;

namespace Tvision2.Core.Engine
{
    public static class TuiEngineExtensions
    {
        public static TvStateManager StateManager(this TuiEngine engine) => engine.GetCustomItem<TvStateManager>();
        public static ITvStoreSelector StoreSelector(this TuiEngine engine) => engine.GetCustomItem<ITvStoreSelector>();
    }
}
