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

        public StatexKeyActionBehavior(IEnumerable<StatexKeyActionCreatorBinder<TControlState>> keyBinders, string storeName, ITvStoreSelector storeSelector)
        {
            _keyBinders = keyBinders.ToList();
            _storeSelector = storeSelector;
            _storeName = storeName;
        }

        public bool Update(BehaviorContext<TControlState> updateContext)
        {
            if (!updateContext.Events.HasKeyboardEvents) return false;

            var keyEvents = updateContext.Events.KeyboardEvents.Where(e => e.IsKeyDown);
            bool updated = false;

            foreach (var evt in keyEvents)
            {
                foreach (var binder in _keyBinders)
                {
                    if (binder.Predicate(evt))
                    {
                        var action = binder.ActionCreator(updateContext.State);
                        _storeSelector.GetStore(_storeName).Dispatch(action);
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
