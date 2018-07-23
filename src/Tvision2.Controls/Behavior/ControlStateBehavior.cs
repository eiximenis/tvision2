using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Components.Behaviors;

namespace Tvision2.Controls.Behavior
{
    public class ControlStateBehavior<TState> : ITvBehavior<TState>
        where TState : IDirtyObject
    {

        private readonly TvControlMetadata _metadata;
        public ControlStateBehavior(TvControlMetadata metadata)
        {
            _metadata = metadata;
        }

        bool ITvBehavior.Update(BehaviorContext updateContext) => Update((BehaviorContext<TState>)updateContext);

        public bool Update(BehaviorContext<TState> updateContext) 
        {
            var isDirty = updateContext.State.IsDirty || _metadata.IsDirty;

            if (isDirty)
            {
                isDirty = false;
                _metadata.Validate();
                updateContext.State.Validate();
                return true;
            }
            return false;
        }
    }
}
