using System;
using System.Collections.Generic;

namespace Tvision2.Core
{

    public interface IOnceActionChain
    {
        void Remove(Guid id);
        void Clear();
        IEnumerable<Guid> Keys { get; }
        Guid AddOnce(IAction action);
    }

    public interface IActionChain : IOnceActionChain
    {
        Guid Add(IAction action);
    }

    public interface IOnceActionChain<TData> : IOnceActionChain
    {
        Guid AddOnce(IAction<TData> action);
        Guid AddOnce(Action<TData> actionFunc);
        Guid AddOnce(Func<TData, ActionResult> actionFunc);
    }


    public interface IActionChain<TData> : IOnceActionChain<TData>, IActionChain
    {
        Guid Add(IAction<TData> action);
        Guid Add(Action<TData> actionFunc);
        Guid Add(Func<TData, ActionResult> actionFunc);

    }
}
