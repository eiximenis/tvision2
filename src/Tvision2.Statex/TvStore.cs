using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Tvision2.Statex
{
    public class TvStore<TState> : ITvStore, ITvConfigurableStore<TState>
        where TState : class
    {

        private TState _currentState;
        private readonly List<Func<TState, TvAction, TState>> _reducers;
        private readonly Func<TState, TState, bool> _dirtyChecker;

        private readonly List<Action<TState>> _subscribers;

        private readonly List<TvAction> _pendingActions;

        public TvStore(TState state) : this(state, DirtyCheckers.ReferenceDirtyChecker<TState>) { }
        public TvStore(TState state, Func<TState, TState, bool> dirtyChecker)
        {
            _pendingActions = new List<TvAction>();
            _currentState = state;
            _reducers = new List<Func<TState, TvAction, TState>>();
            _dirtyChecker = dirtyChecker;
            _subscribers = new List<Action<TState>>();
        }

        ITvConfigurableStore<TState> ITvConfigurableStore<TState>.AddReducer(Func<TState, TvAction, TState> reducer)
        {
            _reducers.Add(reducer);
            return this;
        }

        public void Dispatch(TvAction action)
        {
            _pendingActions.Add(action);
        }

        Type ITvStore.StateType => typeof(TState);
        void ITvStore.Cycle()
        {
            var actions = _pendingActions.ToArray();
            foreach (var action in actions)
            {
                var result = DoDispatchAction(action);
                // TODO: Handle result if errors!
            }

            _pendingActions.Clear();
        }

        private TvActionResult DoDispatchAction(TvAction action)
        {
            var newState = _currentState;
            var errored = TvActionResult.Failed();
            var someError = false;
            foreach (var reducer in _reducers)
            {
                try
                {
                    newState = reducer.Invoke(newState, action);
                }
                catch (Exception ex)
                {
                    errored.AddError(ex);
                    someError = true;
                }
            }

            var isDirty = _dirtyChecker(_currentState, newState);
            if (isDirty)
            {
                _subscribers.ForEach(x => x.Invoke(newState));
            }

            _currentState = newState;

            return someError ? errored : TvActionResult.OK;
        }

        public void Subscribe(Action<TState> action)
        {
            _subscribers.Add(action);
            action.Invoke(_currentState);
        }
    }
}
