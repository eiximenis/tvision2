using System;

namespace Tvision2.Core.Components.Behaviors
{
    public interface IBehaviorMetadata
    {
        bool Created { get; }
        BehaviorSchedule Schedule { get; }
        void CreateBehavior(IServiceProvider sp);

        ITvBehavior Behavior { get; }
    }

    public interface IBehaviorMetadata<T> : IBehaviorMetadata
    {
        new ITvBehavior<T> Behavior { get; }
        IBehaviorMetadata<T> UseScheduler(BehaviorSchedule schedule);
    }
}