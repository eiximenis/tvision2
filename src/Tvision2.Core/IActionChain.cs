using System;
using System.Collections.Generic;

namespace Tvision2.Core
{
    public interface IActionChain
    {
        void Remove(Guid id);
        void Clear();
        IEnumerable<Guid> Keys { get; }
        Guid Add(IAction action);
        Guid AddOnce(IAction action);
 
    }
    public interface IActionChain<TData> : IActionChain
    {

        Guid Add(IAction<TData> action);
        Guid Add(Action<TData> actionFunc);
        Guid Add(Func<TData, ActionResult> actionFunc);
        Guid AddOnce(IAction<TData> action);
        Guid AddOnce(Action<TData> actionFunc);
        Guid AddOnce(Func<TData, ActionResult> actionFunc);
    }
}
