using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Controls;

namespace Tvision2.Core.Engine
{
    public static class ComponentTreeExtensions
    {
        public static void Add(this ComponentTree componentTree, ITvControl control, int zIndex = 0) => componentTree.Add(control.AsComponent(), zIndex);
    }
}
