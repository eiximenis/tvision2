using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tvision2.Controls;
using Tvision2.Controls.Behavior;
using Tvision2.Core.Components.Behaviors;
using Tvision2.Statex.Behaviors;

namespace Tvision2.Statex.Controls.Behaviors
{
    public class StatexBehavior<TControl, TControlState, TStatex> : ITvBehavior<TControlState>
        where TControl: TvControl<TControlState>
        where TControlState : class, IDirtyObject
        where TStatex : class
    {
        private readonly ITvStoreSelector _storeSelector;
        private StatexControlOptions<TControl, TControlState, TStatex> _options;
        private TStatex _currentStatex;

        public StatexBehavior(ITvStoreSelector storeSelector)
        {
            _storeSelector = storeSelector;
            _options = null;
            _currentStatex = null;
        }

        internal void SetupControl(TControl control)
        {
            AddCommandHandlers(control);
            AddKeyHandlers(control);
        }

        private void AddKeyHandlers(TControl control)
        {
            if (_options.KeyActionCreators.Any())
            {
                control.AsComponent().AddBehavior(new StatexKeyActionBehavior<TControlState>(_options.KeyActionCreators, _options.StoreName, _storeSelector));
            }
 
        }

        private void AddCommandHandlers(TControl control)
        {
            foreach (var creator in _options.ActionCreators)
            {
                var commandType = typeof(DelegateCommand<>);
                commandType = commandType.MakeGenericType(creator.CommandType);

                Func<object, Task> @delegate = delegate (object o)
                {
                    var action = creator.ActionCreator.DynamicInvoke(control.State, o);
                    var store = _storeSelector.GetStore(_options.StoreName);
                    store.Dispatch(action as TvAction);
                    return Task.CompletedTask;
                };

                var command = Activator.CreateInstance(commandType, @delegate, creator.Predicate) as ICommand;
                var chain = creator.CommandMember.GetValue(control) as ICommandChain;
                chain.Add(command);
            }
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

        internal void SetOptions(StatexControlOptions<TControl, TControlState, TStatex> options)
        {
            _options = options;
            var store = _storeSelector.GetStore<TStatex>(_options.StoreName);
            store.Subscribe(statex => _currentStatex = statex);
        }
    }
}
