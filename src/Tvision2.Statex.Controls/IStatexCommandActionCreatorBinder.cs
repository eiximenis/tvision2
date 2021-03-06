﻿using System;
using Tvision2.Controls;

namespace Tvision2.Statex.Controls
{


    public interface IStatexCommandActionCreatorBinder<TControlState, TCommandArg>
        where TControlState : IDirtyObject
    {
        IStatexCommandActionFilter<TControlState, TCommandArg> Dispatch(Func<TControlState, TCommandArg, TvAction> actionCreator);
    }
}
