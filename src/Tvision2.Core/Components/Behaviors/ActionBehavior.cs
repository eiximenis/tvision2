using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Components.Props;

namespace Tvision2.Core.Components.Behaviors
{
    public class ActionBehavior : ITvBehavior
    {
        private readonly Func<BehaviorContext, IPropertyBag> _behaviorFunc;

        public ActionBehavior(Func<BehaviorContext, IPropertyBag> behaviorFunc)
        {
            _behaviorFunc = behaviorFunc;
        }

        public IPropertyBag Update(BehaviorContext updateContext) => _behaviorFunc(updateContext);
    }
}
