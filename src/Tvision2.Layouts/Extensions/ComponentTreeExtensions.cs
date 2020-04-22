using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Layouts;
using Microsoft.Extensions.DependencyInjection;

namespace Tvision2.Core.Engine
{
    public static class ComponentTreeExtensions_Layouts
    {
        public static ILayoutManager GetLayoutManager(this ITuiEngine engine) => engine.ServiceProvider.GetService<ILayoutManager>();

        public static void Add(this IComponentTree tree, ITvContainer container)
        {
            tree.Add(container.AsComponent(), opt =>
                opt.ExecuteAfterAdding(engine => 
                    ((LayoutManager)engine.GetLayoutManager()).Add(container)));
        }

        public static bool Remove(this IComponentTree tree, ITvContainer container)
        {
            var removed = tree.Remove(container.AsComponent());
            return removed;
        }
    }
}
