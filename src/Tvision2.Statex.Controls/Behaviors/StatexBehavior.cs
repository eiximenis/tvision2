using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Controls;
using Tvision2.Core.Components.Behaviors;

namespace Tvision2.Statex.Controls.Behaviors
{
    public class StatexBehavior<TControlState, TStatex> : ITvBehavior<TControlState>
        where TControlState : class, IDirtyObject
        where TStatex : class
    {
        private readonly ITvStoreSelector _storeSelector;
        private StatexControlOptions<TControlState, TStatex> _options;
        private TStatex _currentStatex;

        public StatexBehavior(ITvStoreSelector storeSelector)
        {
            _storeSelector = storeSelector;
            _options = null;
            _currentStatex = null;
        }

        bool ITvBehavior.Update(BehaviorContext context) => Update((BehaviorContext<TControlState>)context);

        public bool Update(BehaviorContext<TControlState> updateContext)
        {
            if (_currentStatex == null)
            {
                return false;
            }
            var needsUpdate = _options.UpdateControlState(updateContext.State, _currentStatex);
            _currentStatex = null;
            return needsUpdate;
        }

        internal void SetOptions(StatexControlOptions<TControlState, TStatex> options)
        {
            _options = options;
            var store = _storeSelector.GetStore<TStatex>(_options.StoreName);
            store.Subscribe(statex => _currentStatex = statex);
        }
    }
}
