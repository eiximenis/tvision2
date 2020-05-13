using System;
using System.Collections.Generic;
using static Tvision2.Core.Engine.ComponentTree;

namespace Tvision2.Core.Components.Behaviors
{
    public interface IBehaviorMetadata
    {
        bool Created { get; }
        BehaviorSchedule Schedule { get; }
        void CreateBehavior(IServiceProvider sp);
        ITvBehavior Behavior { get; }

        IEnumerable<OwnedComponentDependencyDescriptor> Dependencies { get; }
    }

    public interface IBehaviorMetadata<T> : IBehaviorMetadata
    {
        new ITvBehavior<T> Behavior { get; }
    }
}