using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Components.Props;
using Tvision2.Core.Engine;

namespace Tvision2.Core.Components.Behaviors
{
    public interface ITvBehavior
    {
        IPropertyBag  Update(BehaviorContext updateContext);
    }
}
