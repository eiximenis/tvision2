using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Components.Behaviors;

namespace Tvision2.Controls.Behavior
{
    public class ControlStateBehavior<TState> : ITvBehavior<TState>
    {
        private readonly TvControlData _controlData;

        public ControlStateBehavior(TvControlData controlData)
        {
            _controlData = controlData;
        }

        bool ITvBehavior.Update(BehaviorContext updateContext) => Update((BehaviorContext<TState>)updateContext);

        public bool Update(BehaviorContext<TState> updateContext) 
        {
            var isDirty = _controlData.IsDirty;

            if (_controlData.IsDirty)
            {
                _controlData.Validate();
                return true;
            }
            return false;
        }
    }
}
