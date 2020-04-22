using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Controls;

namespace Tvision2.Core.Components
{
    public static class ComponentsCollectionExtensions_Controls
    {
        public static void Add(this IComponentsCollection cc, ITvControl ctl) => cc.Add(ctl.AsComponent());
    }
}
