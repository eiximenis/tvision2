using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tvision2.Controls;

namespace Tvision2.Layouts.Grid
{
    public static class GridContainerExtensions
    {
        public static void Add(this IGridContainer container, ITvControl control) => container.Add(control.AsComponent());
    }
}
