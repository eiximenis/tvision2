using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Tvision2.Controls;

namespace Tvision2.Statex.Controls
{
    public class StatexCommandActionCreatorBinder<TControlState> : IStatexCommandActionCreatorBinder<TControlState>
        where TControlState : IDirtyObject
    {
        internal PropertyInfo CommandMember { get; }
        internal Func<TControlState, TvAction> ActionCreator { get; private set; }
        internal Type CommandType { get; }

        public StatexCommandActionCreatorBinder(PropertyInfo commandMember, Type commandType)
        {
            CommandMember = commandMember;
            CommandType = commandType;
        }

        public void Dispatch(Func<TControlState, TvAction> actionCreator)
        {
            ActionCreator = actionCreator;
        }
    }
}
