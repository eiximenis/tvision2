using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Engine;

namespace Tvision2.Core.Components.Behaviors
{

    public interface ITvBehavior { }

    public interface ITvBehavior<T> : ITvBehavior
    {
        bool Update(BehaviorContext<T> updateContext);
    }
}
