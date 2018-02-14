using System;
using System.Collections.Generic;
using System.Text;
using TvConsole;
using Tvision2.Core.Engine;

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
