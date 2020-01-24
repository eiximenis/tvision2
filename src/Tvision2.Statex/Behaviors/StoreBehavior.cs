using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Components.Behaviors;

namespace Tvision2.Statex.Behaviors
{
    public abstract class StoreBehavior<T> : ITvBehavior<T>
    {

        private readonly ITvStoreSelector _storeSelector;
        public StoreBehavior(ITvStoreSelector storeSelector) => _storeSelector = storeSelector;
        public bool Update(BehaviorContext<T> updateContext) => Update(updateContext, _storeSelector);
        protected abstract bool Update(BehaviorContext<T> updateContext, ITvStoreSelector storeSelector);
    }

    public abstract class StoreBehavior<T, T2> : ITvBehavior<T>
        where T2 : class
    {
        private ITvStore<T2> _store;
        private readonly ITvStoreSelector _storeSelector;
        public StoreBehavior(ITvStoreSelector storeSelector)
        {
            _store = storeSelector.GetStore<T2>();
            _storeSelector = storeSelector;
        }

        public bool Update(BehaviorContext<T> updateContext)
        {
            _store = _store ?? _storeSelector.GetStore<T2>();
            return _store != null ? Update(updateContext, _store) : false;
        }
        protected abstract bool Update(BehaviorContext<T> updateContext, ITvStore<T2> store);
    }

}
