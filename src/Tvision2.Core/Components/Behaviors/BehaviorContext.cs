using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Components.Props;
using Tvision2.Core.Engine;

namespace Tvision2.Core.Components.Behaviors
{
    public class BehaviorContext
    {
        public IPropertyBag Properties { get; }
        public ITvDispatcher Dispatcher { get; }

        public TvEventsCollection Events { get; }

        public BehaviorContext(IPropertyBag props, ITvDispatcher dispatcher, TvEventsCollection events)
        {
            Properties = props;
            Dispatcher = dispatcher;
            Events = events;
        }
    }
}
