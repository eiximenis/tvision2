using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Core.Components.Props
{
    public interface ITvProperty
    {
        string Name { get; }
        Type Type { get; }
        bool IsDirty { get; }

        object Value { get; }

        ITvProperty Clone();
    }

    public interface ITvProperty<T> : ITvProperty
    {
        new T Value { get; }
    }
}
