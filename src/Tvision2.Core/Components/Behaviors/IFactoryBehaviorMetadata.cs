using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Core.Components.Behaviors
{
    public interface IFactoryBehaviorMetadata<TB,T> : IBehaviorMetadata<T>
        where TB : ITvBehavior<T>
    {
        void OnCreate(Action<TB> afterCreate);
    }
}
