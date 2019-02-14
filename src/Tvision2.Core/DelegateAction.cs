using System;
using System.Threading.Tasks;

namespace Tvision2.Core
{
    public class DelegateAction<TData> : IAction<TData>
    {
        private readonly Func<TData, Task> _action;
        private readonly Func<TData, bool> _predicate;

        public DelegateAction(Func<TData, Task> action, Func<TData, bool> predicate = null)
        {
            _action = action;
            _predicate = predicate;
        }

        public async Task<bool> Invoke(TData data)
        {
            if (_predicate == null || _predicate(data))
            {
                await _action.Invoke(data);
                return true;
            }

            return false;
        }
    }
}
