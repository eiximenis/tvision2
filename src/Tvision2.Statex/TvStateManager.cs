using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tvision2.Statex
{
    public class TvStateManager : ITvDispatcher, ITvStoreSelector
    {
        private readonly Dictionary<string, ITvStore> _stores;

        public ITvDispatcher Dispatcher => this;

        public TvStateManager()
        {
            _stores = new Dictionary<string, ITvStore>();
        }


        public ITvConfigurableStore<TState> AddStore<TStore, TState>(string name, TStore store)
            where TStore : ITvStore, ITvConfigurableStore<TState>
            where TState : class

        {
            _stores.Add(name, store);
            return store;
        }

        public ITvConfigurableStore<TState> GetStore<TState>(string name) where TState : class => _stores[name] as ITvConfigurableStore<TState>;

        ITvStore<TState> ITvStoreSelector.GetStore<TState>(string name) => _stores[name] as ITvStore<TState>;

        ITvStore<TState> ITvStoreSelector.GetStore<TState>() => _stores.Values.SingleOrDefault(s => s.StateType == typeof(TState)) as ITvStore<TState>;

        internal void DoDispatchAllActions()
        {
            foreach (var store in _stores.Values)
            {
                store.Cycle();
            }

        }

        void ITvDispatcher.Dispatch(TvAction action)
        {
            foreach (var store in _stores.Values)
            {
                store.Dispatch(action);
            }
        }
    }
}
