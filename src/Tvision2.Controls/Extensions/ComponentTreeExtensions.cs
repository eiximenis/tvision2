using Microsoft.Extensions.DependencyInjection;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tvision2.Controls;

namespace Tvision2.Core.Engine
{
    public static class ComponentTreeExtensions_Controls
    {
        public static void InsertAfter(this IComponentTree componentTree, ITvControl control, int position)
        {
            var metadata = componentTree.Add(control.AsComponent());
            var ctree = componentTree.RootControls() as ControlsTree;
            ctree.InsertAfter(control.Metadata, position);
        }
        public static void Add(this IComponentTree componentTree, ITvControl control)
        {
            var metadata = componentTree.Add(control.AsComponent());
            var ctree = componentTree.RootControls() as ControlsTree;
            ctree.Add(control.Metadata);
        }

        public static bool Remove(this IComponentTree componentTree, ITvControl control)
        {
            var removed = componentTree.Remove(control.AsComponent());
            if (removed)
            {
                var ctree = componentTree.RootControls() as ControlsTree;
                ctree.Remove(control.Metadata);
            }

            return removed;
        } 

        public static IControlsTree RootControls(this IComponentTree componentTree) => componentTree.Engine.ServiceProvider.GetService<IControlsTree>();

        public static IEnumerable<TvControlMetadata> OwnedControls(this IComponentTree componentTree)
        {
            var allControls = componentTree.RootControls();
            foreach (var ctlMetadata in allControls.ControlsMetadata)
            {
                var ownedComponent = componentTree.Components.SingleOrDefault(c => c.ComponentId == ctlMetadata.ControlId);
                if (ownedComponent != null)
                {
                    yield return ctlMetadata;
                }
            }
        }
    }
}
