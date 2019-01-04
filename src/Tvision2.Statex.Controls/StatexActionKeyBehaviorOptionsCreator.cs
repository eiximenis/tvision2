using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Controls;
using Tvision2.Controls.Behavior;
using Tvision2.Core.Components.Behaviors;
using Tvision2.Events;

namespace Tvision2.Statex.Controls
{
    class StatexKeyActionCreatorBinder<TControlState> : IStatexKeyActionCreatorBinder<TControlState>
        where TControlState : IDirtyObject
    {


        internal Func<TvConsoleKeyboardEvent, bool> Predicate { get; private set; }
        internal (string StoreName, Func<TControlState, TvAction> Creator) ActionCreator { get; private set; }



        public StatexKeyActionCreatorBinder(Func<TvConsoleKeyboardEvent, bool> keyPredicate)
        {
            Predicate = keyPredicate;
        }


        public void Dispatch(Func<TControlState, TvAction> actionCreator)
        {
            ActionCreator = (null, actionCreator);
        }

        public void DispatchTo(string storeName, Func<TControlState, TvAction> actionCreator)
        {
            ActionCreator = (storeName, actionCreator);
        }


    }
}
