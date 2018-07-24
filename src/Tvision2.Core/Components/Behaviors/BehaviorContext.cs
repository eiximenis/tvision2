using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Engine;
using Tvision2.Core.Render;
using Tvision2.Events;

namespace Tvision2.Core.Components.Behaviors
{
    public class BehaviorContext
    {

        public bool ViewportUpdated { get; private set; }

        public IViewport Viewport { get; private set; }

        public BehaviorContext(TvConsoleEvents events, IViewport viewport)
        {
            Events = events;
            Viewport = viewport;
            ViewportUpdated = false;
        }
        public TvConsoleEvents Events { get; }

        public void UpdateViewport(IViewport newViewport)
        {
            if (!Viewport.Equals(newViewport))
            {
                Viewport = newViewport;
                ViewportUpdated = true;
            }
        }

    }

    public class BehaviorContext<T> : BehaviorContext
    {
        public T State { get; }


        public BehaviorContext(T state, TvConsoleEvents events, IViewport viewport) : base(events, viewport)
        {
            State = state;
        }
    }

}
