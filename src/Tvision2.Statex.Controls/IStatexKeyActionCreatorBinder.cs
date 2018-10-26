using System;
using Tvision2.Controls;

namespace Tvision2.Statex.Controls
{
    public interface IStatexKeyActionCreatorBinder<TControlState>
        where TControlState : IDirtyObject
    {
        void Dispatch(Func<TControlState, TvAction> actionCreator);
    }
}
