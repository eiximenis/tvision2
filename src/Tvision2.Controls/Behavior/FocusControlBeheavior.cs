using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Components.Behaviors;

namespace Tvision2.Controls.Behavior
{
    public class FocusControlBeheavior<TState> : ITvBehavior<TState>
    {
        private readonly ITvBehavior<TState> _inner;
        private readonly TvControlMetadata _metadata;
        public FocusControlBeheavior(TvControlMetadata metadata, ITvBehavior<TState> innerBehavior)
        {
            _inner = innerBehavior;
            _metadata = metadata;
        }

        bool ITvBehavior.Update(BehaviorContext updateContext) => Update((BehaviorContext<TState>)updateContext);
        public bool Update(BehaviorContext<TState> updateContext)
        {
            if (_metadata.IsFocused)
            {
                return _inner.Update(updateContext);
            }

            return false;
        }
    }
}
