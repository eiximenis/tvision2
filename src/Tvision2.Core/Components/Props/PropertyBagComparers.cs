using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PropComparer = System.Func<Tvision2.Core.Components.Props.IPropertyBag, Tvision2.Core.Components.Props.IPropertyBag, bool>;

namespace Tvision2.Core.Components.Props
{
    public static class PropertyBagComparers
    {
        public static PropComparer ReferenceEqualityComparer = (p1, p2) => p1 == p2;
    }
}
