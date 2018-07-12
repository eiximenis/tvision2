using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Components;

namespace Tvision2.Debug
{
    public interface ITvision2DebugOptions
    {
        ITvision2DebugOptions UseDebugFilter(Func<TvComponent, bool> filter);
    }
}
