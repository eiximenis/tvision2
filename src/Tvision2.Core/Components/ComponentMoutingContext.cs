using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Engine;

namespace Tvision2.Core.Components
{
    public struct ComponentMoutingContext
    {
        public ITuiEngine OwnerEngine { get; }
        public TvComponent Component { get; }

        public ComponentMoutingContext(ITuiEngine ownerEngine, TvComponent component)
        {
            OwnerEngine = ownerEngine;
            Component = component;
        }
    }

    public struct ComponentMountingCancellableContext
    {
        public ITuiEngine OwnerEngine { get; }
        public TvComponent Component { get; }

        public bool IsCancelled { get; private set; }

        public bool Cancel() => IsCancelled = true;

        public ComponentMountingCancellableContext(ITuiEngine ownerEngine, TvComponent component)
        {
            OwnerEngine = ownerEngine;
            Component = component;
            IsCancelled = false;
        }

    }
}
