using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Core.Components.Behaviors
{
    public class BehaviorMetadata
    {
        public ITvBehavior Behavior { get; private set; }
        public BehaviorSchedule Schedule { get; private set; }

        public BehaviorMetadata UseScheduler(BehaviorSchedule schedule)
        {
            Schedule = schedule;
            return this;
        }

        public BehaviorMetadata(ITvBehavior behavior)
        {
            Behavior = behavior;
            Schedule = BehaviorSchedule.OncePerFrame;
        }

    }
}
