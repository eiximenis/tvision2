using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Tvision2.Controls
{
    public class DelegateCommand<TData> : ICommand<TData>
    {
        private readonly Func<TData, Task> _action;
        public DelegateCommand(Func<TData, Task> action) => _action = action;

        public Task Invoke(TData data) => _action.Invoke(data);
    }
}
