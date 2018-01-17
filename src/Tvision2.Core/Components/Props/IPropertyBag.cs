using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Core.Components.Props
{
    public interface IPropertyBag : IProps
    {
        void AddPropertyAs<T>(string name, T value);

        IPropertyBag SetValues(object values);
        IPropertyBag SetValues(IPropertyBag values);
    }
}
