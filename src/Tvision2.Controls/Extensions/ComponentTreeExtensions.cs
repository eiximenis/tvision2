using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Controls;

namespace Tvision2.Core.Engine
{
    public static class ComponentTreeExtensions_Controls
    {
        
        public static void InsertAfter(this ComponentTree componentTree, ITvControl control, int position, int zindex = 0)
        {
            var metadata = componentTree.Add(control.AsComponent(), zindex);
            var ctree = componentTree.Controls() as ControlsTree;
            var cdata = new TvControlMetadata(metadata, control);
            ctree.InsertAfter(cdata, position);
        }
        public static void Add(this ComponentTree componentTree, ITvControl control, int zindex = 0)
        {
            var metadata = componentTree.Add(control.AsComponent(), zindex);
            var ctree = componentTree.Controls() as ControlsTree;
            var cdata = new TvControlMetadata(metadata, control);
            ctree.Add(cdata);
        }



        public static IControlsTree Controls(this ComponentTree componentTree) => componentTree.Engine.GetCustomItem<IControlsTree>();
    }
}
