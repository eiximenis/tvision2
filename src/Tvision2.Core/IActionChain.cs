using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tvision2.Core
{
    public interface IActionChain
    {
        Guid Add(IAction action);
        Guid AddOnce(IAction action);
        void Remove(Guid id);
        void Clear();
        IEnumerable<Guid> Keys { get; }
    }
    public interface IActionChain<TData> : IActionChain
    {
        Guid Add(IAction<TData> action);
        Guid AddOnce(IAction<TData> action);
        Task Invoke(TData item);
        Guid Add(Func<TData, Task<bool>> actionFunc);
        Guid AddOnce(Func<TData, Task<bool>> commandFunc);
    }
}
