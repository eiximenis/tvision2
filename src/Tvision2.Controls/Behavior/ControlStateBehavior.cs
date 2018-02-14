using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Components.Behaviors;

namespace Tvision2.Controls.Behavior
{
    public class ControlStateBehavior<TState> : ITvBehavior<TState>
        where TState : IControlState
    {
        private readonly TState _state;
        public ControlStateBehavior(TState state)
        {
            _state = state;
        }

        public bool Update(BehaviorContext<TState> updateContext) 
        {
            if (_state.IsDirty)
            {
                _state.Reset();
                return true;
            }
            return false;
        }
    }
}
