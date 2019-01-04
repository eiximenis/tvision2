using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Tvision2.Controls;
using Tvision2.Events;

namespace Tvision2.Statex.Controls
{
    public class StatexControlOptions<TControl, TControlState, TStatex>
        where TControlState : class, IDirtyObject
        where TControl: TvControl<TControlState>

    {
        private Action<TStatex, TControlState> _controlStateUpdater;
        private List<StatexCommandActionCreatorBinder> _actionCreators;

        private List<StatexKeyActionCreatorBinder<TControlState>> _keyActionCreators;

        internal string StoreName { get; private set; }
        internal IEnumerable<StatexCommandActionCreatorBinder> ActionCreators => _actionCreators;
        internal IEnumerable<StatexKeyActionCreatorBinder<TControlState>> KeyActionCreators => _keyActionCreators;

        public StatexControlOptions()
        {
            _actionCreators = new List<StatexCommandActionCreatorBinder>();
            _keyActionCreators = new List<StatexKeyActionCreatorBinder<TControlState>>();
        }

        public void UseDefaultStore(string storeName)
        {
            StoreName = storeName;
        }

        public void UseStatexTransformation(Action<TStatex, TControlState> controlStateUpdater)
        {
            _controlStateUpdater = controlStateUpdater;
        }

        public IStatexCommandActionCreatorBinder<TControlState, TCommandArg> On<TCommandArg>(Expression<Func<TControl, ICommandChain<TCommandArg>>> commandSelector)
        {
            if (commandSelector.Body is MemberExpression expr && expr.Member is PropertyInfo pi)
            {
                var creatorBinder = new StatexCommandActionCreatorBinder<TControlState, TCommandArg>(pi, typeof(TCommandArg));
                _actionCreators.Add(creatorBinder);
                return creatorBinder;
            }

            throw new ArgumentException($"{nameof(commandSelector)} must be a lambda referring to a ICommandChain.");
        }

        public IStatexKeyActionCreatorBinder<TControlState> OnKeyEvent(Func<TvConsoleKeyboardEvent, bool> keyPredicate)
        {
            var keyOptionsCreator = new StatexKeyActionCreatorBinder<TControlState>(keyPredicate);
            _keyActionCreators.Add(keyOptionsCreator);
            return keyOptionsCreator;
        }

        internal bool UpdateControlState(TControlState controlState, TStatex statex)
        {
            _controlStateUpdater?.Invoke(statex, controlState);
            return controlState.IsDirty;
        }

    }
}
