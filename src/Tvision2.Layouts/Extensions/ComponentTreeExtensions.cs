using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Layouts;
using Microsoft.Extensions.DependencyInjection;

namespace Tvision2.Core.Engine
{
    public static class ComponentTreeExtensions_Layouts
    {
        public static void Add(this IComponentTree tree, ITvContainer container)
        {
            var layoutManager = tree.Layouts() as LayoutManager;
            layoutManager.Add(container);
            tree.Add(container.AsComponent());
        }

        public static bool Remove(this IComponentTree tree, ITvContainer container)
        {
            var removed = tree.Remove(container.AsComponent());
            if (removed)
            {
                var layoutManager = tree.Layouts() as LayoutManager;
                layoutManager.Remove(container);
            }

            return removed;
        }

        public static ILayoutManager Layouts(this IComponentTree componentTree) => componentTree.Engine.ServiceProvider.GetService<ILayoutManager>();
    }
}
