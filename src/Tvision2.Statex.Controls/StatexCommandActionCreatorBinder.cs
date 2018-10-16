using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Tvision2.Controls;

namespace Tvision2.Statex.Controls
{


    public class StatexCommandActionCreatorBinder
    {
        internal PropertyInfo CommandMember { get; }
        internal Type CommandType { get; }

        internal Delegate ActionCreator { get; private protected set; }

        public StatexCommandActionCreatorBinder(PropertyInfo commandMember, Type commandType)
        {
            CommandMember = commandMember;
            CommandType = commandType;
        }
    }
    public class StatexCommandActionCreatorBinder<TControlState, TCommandArg> : StatexCommandActionCreatorBinder, IStatexCommandActionCreatorBinder<TControlState, TCommandArg>
        where TControlState : IDirtyObject
    {
        
        public StatexCommandActionCreatorBinder(PropertyInfo commandMember, Type commandType) : base (commandMember, commandType)
        {
        }

        public void Dispatch(Func<TControlState, TCommandArg, TvAction> actionCreator)
        {
            ActionCreator = actionCreator;
        }
    }
}
