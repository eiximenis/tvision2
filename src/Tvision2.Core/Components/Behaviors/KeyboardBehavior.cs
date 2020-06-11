using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Engine;
using Tvision2.Events;

namespace Tvision2.Core.Components.Behaviors
{
    public abstract class KeyboardBehavior<T> : ITvBehavior<T>
    {
        public bool Update(BehaviorContext<T> updateContext)
        {
            var events = updateContext.Events.KeyboardEvents;
            var props = updateContext.State;
            var updated = false;

            foreach (var evt in events)
            { 
                updated = OnKeyPress(evt, updateContext) || updated;
            }

            return updated;
        }

        protected abstract bool OnKeyPress(TvConsoleKeyboardEvent evt, BehaviorContext<T> updateContext);
    }
}
