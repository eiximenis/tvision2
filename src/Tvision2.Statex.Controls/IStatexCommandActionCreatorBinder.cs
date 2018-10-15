using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Controls;

namespace Tvision2.Statex.Controls
{
    public interface IStatexCommandActionCreatorBinder<TControlState>
        where TControlState : IDirtyObject
    {
        void Dispatch(Func<TControlState, TvAction> actionCreator);
    }
}
