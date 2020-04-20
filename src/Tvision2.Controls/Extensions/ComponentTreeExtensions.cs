using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tvision2.Controls;

namespace Tvision2.Core.Engine
{
    public static class ComponentTreeExtensions_Controls
    {
        public static void Add(this IComponentTree componentTree, ITvControl control, Action<ITuiEngine> afterAdd = null)
        {
            componentTree.Add(control.AsComponent(), afterAdd);
        }

        public static void AddAfter(this IComponentTree componentTree, ITvControl control, ITvControl controlBefore, Action<ITuiEngine> afterAddAction = null)
        {
            componentTree.AddAfter(control.AsComponent(), controlBefore.AsComponent(), afterAddAction);
        }

        public static void AddAsChild(this IComponentTree componentTree, ITvControl control, ITvControl parent, Action<ITuiEngine> afterAddAction = null)
        {
            componentTree.AddAsChild(control.AsComponent(), parent.AsComponent(), afterAddAction);
        }

        public static bool Remove(this IComponentTree componentTree, ITvControl control)
        {
            return componentTree.Remove(control.AsComponent());
        } 


        public static IEnumerable<TvControlMetadata> OwnedControls(this IComponentTree componentTree, IControlsTree controlsTree)
        {
            foreach (var ctlMetadata in controlsTree.ControlsMetadata)
            {
                var ownedComponent = componentTree.Components.SingleOrDefault(c => c.ComponentId == ctlMetadata.ControlId);
                if (ownedComponent != null)
                {
                    yield return ctlMetadata;
                }
            }
        }

        public static IControlsTree GetControlsTree(this ITuiEngine engine) => engine.ServiceProvider.GetService<IControlsTree>();
    }
}
