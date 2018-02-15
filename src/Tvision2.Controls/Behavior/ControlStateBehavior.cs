using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Components.Behaviors;

namespace Tvision2.Controls.Behavior
{
    public class ControlStateBehavior<TState> : ITvBehavior<TState>
    {
        private readonly IControlData _controlData;
        public ControlStateBehavior(IControlData controlData)
        {
            _controlData = controlData;
        }

        public bool Update(BehaviorContext<TState> updateContext) 
        {
            if (_controlData.IsDirty)
            {
                _controlData.Reset();
                return true;
            }
            return false;
        }
    }
}
