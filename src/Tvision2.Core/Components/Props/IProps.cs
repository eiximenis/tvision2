using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Core.Components.Props
{
    public interface IProps
    {
        bool HasProperty(string name);
        T GetPropertyAs<T>(string name);
        Func<IPropertyBag, IPropertyBag, bool> EqualityComparer { get; }
        bool IsEqualTo(IPropertyBag other);
        IEnumerable<ITvProperty> Properties { get; }
    }
}
