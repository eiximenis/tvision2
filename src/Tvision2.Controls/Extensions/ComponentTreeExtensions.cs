using Microsoft.Extensions.DependencyInjection;
using Tvision2.Controls;

namespace Tvision2.Core.Engine
{
    public static class ComponentTreeExtensions_Controls
    { 
      public static void InsertAfter(this ComponentTree componentTree, ITvControl control, int position)
        {
            var metadata = componentTree.Add(control.AsComponent());
            var ctree = componentTree.Controls() as ControlsTree;
            ctree.InsertAfter(control.Metadata, position);
        }
        public static void Add(this ComponentTree componentTree, ITvControl control)
        {
            var metadata = componentTree.Add(control.AsComponent());
            var ctree = componentTree.Controls() as ControlsTree;
            ctree.Add(control.Metadata);
        }

        public static IControlsTree Controls(this ComponentTree componentTree) => componentTree.Engine.ServiceProvider.GetService<IControlsTree>();
    }
}
