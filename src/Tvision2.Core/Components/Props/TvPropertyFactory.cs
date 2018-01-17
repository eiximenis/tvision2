using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Core.Components.Props
{
    public class TvPropertyFactory
    {
        public static ITvProperty CreateImmutableFromType(Type type, string name, object value)
        {
            var valueType = value.GetType();

            if (type != valueType)
            {
                throw new ArgumentException($"Value of type is {type.Name} but value is of type {valueType.Name}.");
            }

            var propClassType = typeof(ImmutableTvProperty<>);
            var typedPropClassType = propClassType.MakeGenericType(type);

            var ctor = typedPropClassType.GetConstructor(new[] { typeof(string), type });
            var tvProperty = ctor.Invoke(new[] { name, value }) as ITvProperty;

            return tvProperty;
        }

    }
}
