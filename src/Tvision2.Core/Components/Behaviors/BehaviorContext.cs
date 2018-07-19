using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Engine;
using Tvision2.Events;

namespace Tvision2.Core.Components.Behaviors
{
    public class BehaviorContext
    {
        public BehaviorContext(TvConsoleEvents events)
        {
            Events = events;
        }
        public TvConsoleEvents Events { get; }
    }

    public class BehaviorContext<T> : BehaviorContext
    {
        public T State { get; }


        public BehaviorContext(T state, TvConsoleEvents events) : base(events)
        {
            State = state;
        }
    }

}
