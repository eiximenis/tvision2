using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Components;

namespace Tvision2.Debug
{
    class Tvision2DebugOptions : ITvision2DebugOptions
    {
        public Func<TvComponent, bool> ComponentDebuggable { get; private set; }

        public Tvision2DebugOptions()
        {
            ComponentDebuggable = (_) => true;
        }

        ITvision2DebugOptions ITvision2DebugOptions.UseDebugFilter(Func<TvComponent, bool> filter)
        {
            ComponentDebuggable = filter;
            return this;
        }
    }
}
