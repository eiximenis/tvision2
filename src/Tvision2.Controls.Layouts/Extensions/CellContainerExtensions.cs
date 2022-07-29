using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tvision2.Controls;

namespace Tvision2.Layouts.Grid
{
    public static class CellContainerExtensions
    {
        public static void Add(this ICellContainer container, ITvControl control) => container.Add(control.AsComponent());
    }
}
