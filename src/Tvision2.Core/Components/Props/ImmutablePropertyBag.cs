using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Tvision2.Core.Components.Props
{
    public class ImmutablePropertyBag : TvPropertyBag
    {
        public ImmutablePropertyBag() : this(null)
        {
        }

        public ImmutablePropertyBag(Func<IPropertyBag, IPropertyBag, bool> equalityComparer) : base(equalityComparer ?? PropertyBagComparers.ReferenceEqualityComparer)
        {
        }

        public override void AddPropertyAs<T>(string name, T value)
        {
            _properties.Add(name, new ImmutableTvProperty<T>(name, value));
        }

        public override void AddPropertyIfNotExists<T>(string name, T value)
        {
            if (!_properties.ContainsKey(name))
            {
                AddPropertyAs(name, value);
            }
        }

        public override IPropertyBag SetValues(IPropertyBag values)
        {
            var newBag = new ImmutablePropertyBag();
            foreach (var prop in newBag.Properties)
            {
                newBag._properties.Add(prop.Name, prop.Clone());
            }

            return newBag;
        }

        public override IPropertyBag SetValues(object values)
        {
            var newBag = new ImmutablePropertyBag();
            foreach (var prop in _properties)
            {
                newBag._properties.Add(prop.Key, prop.Value);
            }
            newBag.FillFromObject(values);
            return newBag;
        }

        private void FillFromObject(object props)
        {
            var properties = props.GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanRead && !p.GetCustomAttributes<PropIgnoreAttribute>().Any());

            foreach (var property in properties)
            {
                var tvProp = TvPropertyFactory.CreateImmutableFromType(property.PropertyType, property.Name, property.GetValue(props));
                if (_properties.ContainsKey(tvProp.Name))
                {
                    _properties[tvProp.Name] = tvProp;
                }
                else
                {
                    _properties.Add(tvProp.Name, tvProp);
                }
            }
        }

        public static ImmutablePropertyBag FromObject(object props)
        {
            var bag = new ImmutablePropertyBag();
            if (props != null)
            {
                bag.FillFromObject(props);
            }
            return bag;
        }
    }
}
