using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Components;

namespace Tvision2.Controls
{
    public interface ITvControl
    {
        TvComponent AsComponent();
    }

    public interface ITvControl<TState> : ITvControl
    {
        TState State { get; }
        new TvComponent<TState> AsComponent();
    }

}
