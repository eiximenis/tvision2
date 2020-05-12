using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Tvision2.Core.Engine;
using Tvision2.Core.Render;
using Tvision2.Events;

namespace Tvision2.Core.Components.Behaviors
{
    public readonly ref struct BehaviorContext<T>
    {
        public IViewport Viewport { get; }

        public ITvConsoleEvents Events { get; }
        public T State { get; }


        public ComponentLocator ComponentLocator { get; }

        public BehaviorContext(T state, ITvConsoleEvents events, IViewport viewport, in ComponentLocator locator)
        {
            Events = events;
            Viewport = viewport;
            State = state;
            ComponentLocator = locator;
        }
    }
}
