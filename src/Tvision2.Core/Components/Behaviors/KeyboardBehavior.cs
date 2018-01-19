using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Components.Props;
using Tvision2.Core.Engine;

namespace Tvision2.Core.Components.Behaviors
{
    public abstract class KeyboardBehavior : ITvBehavior
    {
        public IPropertyBag Update(BehaviorContext updateContext)
        {
            var events = updateContext.Events.KeyPressEvents();
            var props = updateContext.Properties;
            foreach (var evt in events)
            {
                props = OnKeyDown(evt, updateContext, props) ?? props;
            }
            return props;
        }

        protected abstract IPropertyBag OnKeyDown(KeyEvent evt, BehaviorContext updateContext, IPropertyBag currentProperties);
    }
}
