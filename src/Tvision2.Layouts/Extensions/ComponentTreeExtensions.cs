using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Layouts;

namespace Tvision2.Core.Engine
{
    public static class ComponentTreeExtensions_Layouts
    {
        public static void Add(this ComponentTree tree, ITvContainer container)
        {
            tree.Add(container.AsComponent());
        }
    }
}
