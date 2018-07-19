using System;

namespace Tvision2.Core.Components.Behaviors
{
    public interface IBehaviorMetadata
    {
        bool Created { get; }
        BehaviorSchedule Schedule { get; }
        ITvBehavior Behavior { get; }

        void CreateBehavior(IServiceProvider sp);
    }

    public interface IBehaviorMetadata<T> : IBehaviorMetadata
    {
        IBehaviorMetadata<T> UseScheduler(BehaviorSchedule schedule);
    }
}