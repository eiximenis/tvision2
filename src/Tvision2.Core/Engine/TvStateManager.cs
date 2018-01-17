using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tvision2.Core.Stores;

namespace Tvision2.Core.Engine
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
            if (_stores.Values.Select(s => s.StateType).Contains(store.StateType))
            {
                throw new ArgumentException($"There is already one store with StateType: {store.StateType.Name}");
            }
            _stores.Add(name, store);
            return store;
        }

        public ITvConfigurableStore<TState> GetStore<TState>(string name) where TState : class => _stores[name] as ITvConfigurableStore<TState>;

        ITvStore<TState> ITvStoreSelector.GetStore<TState>(string name) => _stores[name] as ITvStore<TState>;

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
