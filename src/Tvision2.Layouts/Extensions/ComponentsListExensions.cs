using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Layouts;

namespace Tvision2.Core.Components
{
    public static class ComponentsListExensions_Layouts
    {
        public static void Add(this ICellContainer cc, ITvContainer ctr) => cc.Add(ctr.AsComponent());
    }
}
