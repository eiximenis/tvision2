using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Core.Components.Behaviors
{
    public class ActionBehavior<T> : ITvBehavior<T>
    {
        private readonly Func<BehaviorContext<T>, bool> _behaviorFunc;

        public ActionBehavior(Func<BehaviorContext<T>, bool> behaviorFunc)
        {
            _behaviorFunc = behaviorFunc;
        }


        bool ITvBehavior.Update(BehaviorContext updateContext) => Update((BehaviorContext<T>)updateContext);

        public bool Update(BehaviorContext<T> updateContext) => _behaviorFunc(updateContext);
    }
}
