using System;
using System.Collections.Generic;
using System.Text;
using TvConsole;
using Tvision2.Core.Engine;

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
                if (evt.IsKeyDown)
                {
                    updated = OnKeyDown(evt, updateContext) || updated;
                }
                else
                {
                    updated = OnKeyUp(evt, updateContext) || updated;
                }
            }

            return updated;
        }

        protected abstract bool OnKeyDown(TvConsoleKeyboardEvent evt, BehaviorContext<T> updateContext);
        protected abstract bool OnKeyUp(TvConsoleKeyboardEvent evt, BehaviorContext<T> updateContext);
    }
}
