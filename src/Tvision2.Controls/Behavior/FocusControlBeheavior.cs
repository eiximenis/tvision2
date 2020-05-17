using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tvision2.Core.Components.Behaviors;
using Tvision2.Core.Engine;

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

        public bool Update(BehaviorContext<TState> updateContext)
        {
            if (HasFocusOnTree(updateContext))
            {
                return _inner.Update(updateContext);
            }

            return false;
        }

        private bool HasFocusOnTree(BehaviorContext<TState> updateContext)
        {
            return _metadata.IsFocused || updateContext.ComponentLocator.DescendantControls().Any(m => m.IsFocused);
        }
    }
}
