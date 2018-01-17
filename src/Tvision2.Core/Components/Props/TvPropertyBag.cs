using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Tvision2.Core.Components.Props
{
    public abstract class TvPropertyBag : IPropertyBag
    {
        protected readonly Dictionary<string, ITvProperty> _properties;
        public Func<IPropertyBag, IPropertyBag, bool> EqualityComparer { get; }

        public TvPropertyBag() : this(null)
        {   
        }
        
        public TvPropertyBag(Func<IPropertyBag, IPropertyBag, bool> equalityComparer)
        {
            _properties = new Dictionary<string, ITvProperty>();
            EqualityComparer = equalityComparer ?? PropertyBagComparers.ReferenceEqualityComparer;
        }

        public IEnumerable<ITvProperty> Properties => _properties.Values;

        public override sealed bool Equals(object obj) => obj is TvPropertyBag ? IsEqualTo((TvPropertyBag)obj) : false;

        public bool IsEqualTo(IPropertyBag other) => EqualityComparer == other.EqualityComparer &&  EqualityComparer(this, other);


        public bool HasProperty(string name) => _properties.ContainsKey(name);

        public T GetPropertyAs<T>(string name) 
        {
            if (string.IsNullOrEmpty(name)) return default(T);
            return _properties.ContainsKey(name) ? ((ITvProperty<T>)_properties[name]).Value : default(T);
        }


        public abstract void  AddPropertyAs<T>(string name, T value);
        public abstract void AddPropertyIfNotExists<T>(string name, T value);

        public abstract IPropertyBag SetValues(object values);
        public abstract IPropertyBag SetValues(IPropertyBag values);

    }
}