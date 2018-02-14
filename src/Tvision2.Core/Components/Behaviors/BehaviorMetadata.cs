using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Core.Components.Behaviors
{
    public class BehaviorMetadata<T>
    {
        public ITvBehavior<T> Behavior { get; private set; }
        public BehaviorSchedule Schedule { get; private set; }

        public BehaviorMetadata<T> UseScheduler(BehaviorSchedule schedule)
        {
            Schedule = schedule;
            return this;
        }

        public BehaviorMetadata(ITvBehavior<T> behavior)
        {
            Behavior = behavior;
            Schedule = BehaviorSchedule.OncePerFrame;
        }

    }
}
