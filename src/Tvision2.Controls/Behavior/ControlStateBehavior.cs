using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Components.Behaviors;
using Tvision2.Core.Components.Props;

namespace Tvision2.Controls.Behavior
{
    public class ControlStateBehavior<TState> : ITvBehavior
        where TState : IControlState
    {
        private readonly TState _state;
        public ControlStateBehavior(TState state)
        {
            _state = state;
        }

        public IPropertyBag Update(BehaviorContext updateContext)
        {
            if (_state.IsDirty)
            {
                var newProps = _state.GetNewProperties(updateContext.Properties);
                _state.Reset();
                return newProps;
            }

            return null;
        }
    }
}
