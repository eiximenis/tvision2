using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Engine;

namespace Tvision2.Core.Hooks
{
    public class HookContext
    {
        public IComponentTree Components { get; }
        public ICustomItemsProvider ItemsProvider { get; }

        public HookContext(TuiEngine engine)
        {
            Components = engine.UI;
            ItemsProvider = engine;
        }
    }
}
