using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Core.Components.Behaviors
{
    public class BehaviorMetadata
    {
        public ITvBehavior Behavior { get; }
        public BehaviorSchedule Schedule { get; }

        public BehaviorMetadata(ITvBehavior behavior)
        {
            Behavior = behavior;
            Schedule = BehaviorSchedule.OncePerFrame;
        }

    }
}
