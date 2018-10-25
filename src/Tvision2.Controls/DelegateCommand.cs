using System;
using System.Threading.Tasks;

namespace Tvision2.Controls
{
    public class DelegateCommand<TData> : ICommand<TData>
    {
        private readonly Func<TData, Task> _action;
        private readonly Func<TData, bool> _predicate;

        public DelegateCommand(Func<TData, Task> action, Func<TData, bool> predicate = null)
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
