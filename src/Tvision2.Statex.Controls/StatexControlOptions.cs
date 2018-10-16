using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Tvision2.Controls;

namespace Tvision2.Statex.Controls
{
    public class StatexControlOptions<TControl, TControlState, TStatex>
        where TControlState : class, IDirtyObject
        where TControl: TvControl<TControlState>

    {
        private Action<TStatex, TControlState> _controlStateUpdater;
        private List<StatexCommandActionCreatorBinder> _actionCreators;

        internal string StoreName { get; private set; }
        internal IEnumerable<StatexCommandActionCreatorBinder> ActionCreators => _actionCreators;

        public StatexControlOptions()
        {
            _actionCreators = new List<StatexCommandActionCreatorBinder>();
        }

        public void UseStore(string storeName)
        {
            StoreName = storeName;
        }

        public void UseStatexTransformation(Action<TStatex, TControlState> controlStateUpdater)
        {
            _controlStateUpdater = controlStateUpdater;
        }

        public IStatexCommandActionCreatorBinder<TControlState, TCommandArg> On<TCommandArg>(Expression<Func<TControl, ICommand<TCommandArg>>> commandSelector)
        {
            if (commandSelector.Body is MemberExpression expr && expr.Member is PropertyInfo pi)
            {
                var creatorBinder = new StatexCommandActionCreatorBinder<TControlState, TCommandArg>(pi, typeof(TCommandArg));
                _actionCreators.Add(creatorBinder);
                return creatorBinder;
            }

            throw new ArgumentException($"{nameof(commandSelector)} must be a lambda referring to a ICommand.");
        }

        internal bool UpdateControlState(TControlState controlState, TStatex statex)
        {
            _controlStateUpdater?.Invoke(statex, controlState);
            return controlState.IsDirty;
        }

    }
}
