using System.Collections.Generic;
using System.Linq;
using Tvision2.Controls;
using Tvision2.Core.Components.Behaviors;
using Tvision2.Statex.Controls;

namespace Tvision2.Statex.Behaviors
{
    class StatexKeyActionBehavior<TControlState> : ITvBehavior<TControlState>
        where TControlState : IDirtyObject
    {

        private readonly List<StatexKeyActionCreatorBinder<TControlState>> _keyBinders;
        private readonly ITvStoreSelector _storeSelector;
        private readonly string _storeName;
        private readonly TvControlMetadata _metadata;

        public StatexKeyActionBehavior(IEnumerable<StatexKeyActionCreatorBinder<TControlState>> keyBinders, string storeName, ITvStoreSelector storeSelector, TvControlMetadata metadata)
        {
            _keyBinders = keyBinders.ToList();
            _storeSelector = storeSelector;
            _storeName = storeName;
            _metadata = metadata;
        }

        public bool Update(BehaviorContext<TControlState> updateContext)
        {
            if (!updateContext.Events.HasKeyboardEvents || !_metadata.IsFocused) return false;
            
            var keyEvents = updateContext.Events.KeyboardEvents.Where(e => e.IsKeyDown);
            bool updated = false;

            foreach (var evt in keyEvents)
            {
                foreach (var binder in _keyBinders)
                {
                    if (binder.Predicate(evt))
                    {
                        var storeName = string.IsNullOrEmpty(binder.ActionCreator.StoreName) ? _storeName : binder.ActionCreator.StoreName;
                        var action = binder.ActionCreator.Creator(updateContext.State);
                        _storeSelector.GetStore(storeName).Dispatch(action);
                        evt.Handle();
                        updated = true;
                    }
                }
            }

            return updated;
        }

        bool ITvBehavior.Update(BehaviorContext updateContext) => Update(updateContext as BehaviorContext<TControlState>);
    }




}
