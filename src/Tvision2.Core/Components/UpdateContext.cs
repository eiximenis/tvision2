using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Engine;
using Tvision2.Events;

namespace Tvision2.Core.Components
{
    public ref struct UpdateContext
    {
        public ITvConsoleEvents Events { get; }
        public ComponentLocator ComponentLocator { get; }

        public UpdateContext(ITvConsoleEvents events, in ComponentLocator locator)
        {
            Events = events;
            ComponentLocator = locator;
        }


    }
}
