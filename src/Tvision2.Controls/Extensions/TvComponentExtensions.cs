using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Controls;

namespace Tvision2.Core.Components
{
    public static class TvComponentExtensions
    {
        public static void ApplyDefinition (this TvComponent cmp, IComponentDefinition definition)
        {
            foreach (var behavior in definition.Behaviors)
            {
                cmp.AddBehavior(behavior);
            }

            foreach (var drawer in definition.Drawers)
            {
                cmp.AddDrawer(drawer);
            }

        }
    }
}
