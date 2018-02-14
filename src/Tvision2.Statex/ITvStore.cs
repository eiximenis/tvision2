using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Statex
{
    public interface ITvStore
    {
        Type StateType { get; }
        void Dispatch(TvAction action);

        void Cycle();
    }

    public interface ITvStore<TState> 
        where TState : class
    {
        void Subscribe(Action<TState> action);
    }

    public interface ITvConfigurableStore<TState> : ITvStore<TState>
        where TState : class
    {
        ITvConfigurableStore<TState> AddReducer(Func<TState, TvAction, TState> reducer);
    }
}
