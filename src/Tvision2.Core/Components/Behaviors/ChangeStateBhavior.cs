using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Core.Components.Behaviors
{
    internal class ChangeStateBhavior<T> : ITvBehavior<T>
    {
        private readonly Func<T,T> _newStateGenerator;
        private readonly TvComponent<T> _component;

        public ChangeStateBhavior(Func<T, T> newStateGenerator, TvComponent<T> component)
        {
            _newStateGenerator = newStateGenerator;
            _component = component;
        }

        public bool Update(BehaviorContext<T> ctx)
        {
            var newState = _newStateGenerator(ctx.State);
            _component.SetState(newState);
            return _component.NeedToRedraw != RedrawNeededAction.None;
        }
    }
}
