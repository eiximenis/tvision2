using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Tvision2.Statex
{
    public interface ITvStore
    {
        Type StateType { get; }
        void Dispatch(TvAction action);

        Task Cycle();
    }

    public interface ITvStore<TState> : ITvStore
        where TState : class
    {
        void Subscribe(Action<TState> action);
    }

    public interface ITvConfigurableStore<TState> : ITvStore<TState>
        where TState : class
    {
        ITvConfigurableStore<TState> AddReducer(Func<TState, TvAction, Task<TState>> reducer);
    }
}
