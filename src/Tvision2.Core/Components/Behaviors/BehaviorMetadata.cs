using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Core.Components.Behaviors
{
    public class BehaviorMetadata<T> : IBehaviorMetadata<T>
    {
        public ITvBehavior Behavior { get; private set; }
        public BehaviorSchedule Schedule { get; private set; }

        public bool Created => true;

        void IBehaviorMetadata.CreateBehavior(IServiceProvider sp) { }          // No need to do anything as behavior is always creeated.

        public IBehaviorMetadata<T> UseScheduler(BehaviorSchedule schedule)
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
