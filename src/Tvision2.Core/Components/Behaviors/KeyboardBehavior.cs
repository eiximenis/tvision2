using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Engine;
using Tvision2.Events;

namespace Tvision2.Core.Components.Behaviors
{
    public abstract class KeyboardBehavior<T> : ITvBehavior<T>
    {
        bool ITvBehavior.Update(BehaviorContext updateContext) => Update((BehaviorContext<T>)updateContext);
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

                // TODO: Need to know if event is handled (update itself is not enought)

            }

            return updated;
        }

        protected abstract bool OnKeyDown(TvConsoleKeyboardEvent evt, BehaviorContext<T> updateContext);
        protected abstract bool OnKeyUp(TvConsoleKeyboardEvent evt, BehaviorContext<T> updateContext);
    }
}
