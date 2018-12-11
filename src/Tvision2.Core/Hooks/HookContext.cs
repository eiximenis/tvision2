using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Engine;

namespace Tvision2.Core.Hooks
{
    public class HookContext
    {
        public IComponentTree ComponentTree { get; }
        public HookContext(ITuiEngine engine)
        {
            ComponentTree = engine.UI;
        }
    }
}
