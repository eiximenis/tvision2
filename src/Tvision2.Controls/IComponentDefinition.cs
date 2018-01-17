using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Components.Behaviors;
using Tvision2.Core.Components.Draw;

namespace Tvision2.Controls
{
    public interface IComponentDefinition
    {
        IEnumerable<ITvBehavior> Behaviors { get; }
        IEnumerable<ITvDrawer> Drawers { get; }
    }
}
