using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Engine;
using Tvision2.Events;

namespace Tvision2.Core.Components.Behaviors
{
    public class BehaviorContext<T>
    {
        public T State { get; }

        public TvConsoleEvents Events { get; }

        public BehaviorContext(T state, TvConsoleEvents events)
        {
            State = state;
            Events = events;
        }
    }

}
