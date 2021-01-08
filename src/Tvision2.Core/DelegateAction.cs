using System;
using System.Threading.Tasks;

namespace Tvision2.Core
{

    public class DelegateAction<TData> : IAction<TData>
    {
        private readonly Func<TData, ActionResult> _action;
        private readonly Func<TData, bool> _predicate;

        public DelegateAction(Func<TData, ActionResult> action, Func<TData, bool>? predicate = null)
        {
            _action = action;
            _predicate = predicate;
        }

        public DelegateAction(Action<TData> action, Func<TData, bool>? predicate = null)
        {
            _action = d =>
            {
                action(d);
                return ActionResult.Continue;
            };
            _predicate = predicate;
        }

        public ActionResult Invoke(TData data)
        {
            if (_predicate == null || _predicate(data))
            {
                return _action.Invoke(data);
            }

            return ActionResult.Continue;
        }
    }
}
