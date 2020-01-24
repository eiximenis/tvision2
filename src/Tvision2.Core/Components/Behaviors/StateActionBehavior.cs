using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Core.Components.Behaviors
{
    internal class StateActionBehavior<T> : ITvBehavior<T>
    {
        private readonly Func<T, bool> _action;

        public StateActionBehavior(Func<T, bool> action)
        {
            _action = action;
        }
        public bool Update(BehaviorContext<T> updateContext)
        {
            return _action(updateContext.State);
        }
    }
}
