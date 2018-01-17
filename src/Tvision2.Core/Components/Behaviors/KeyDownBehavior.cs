using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tvision2.Core.Components.Props;
using Tvision2.Core.Engine;

namespace Tvision2.Core.Components.Behaviors
{
    public class KeyDownBehavior : OptionsBehavior<KeyBehaviorOptions>
    {
        public KeyDownBehavior(Action<KeyBehaviorOptions> optionsAction) : base(optionsAction)
        {
        }

        public override IPropertyBag Update(BehaviorContext updateContext)
        {
            if (updateContext.Events == null) { return null; }

            var keyEvents = updateContext.Events.KeyPressEvents();
            foreach (var ke in keyEvents)
            {
                if (Options.Keys.Any(k => k == ke.KeyInfo.Key))
                {
                    Options.Action?.Invoke(ke.KeyInfo.Key);
                }
            }
            return null;
        }

    }
}
