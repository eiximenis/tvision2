using System;

namespace Tvision2.Core.Components.Props
{

    public class ImmutableTvProperty<T> : TvProperty, ITvProperty<T>
    {
        public T Value { get; }

        object ITvProperty.Value => Value;

        public override bool IsDirty => true;

        public ImmutableTvProperty(string name, T value) : base(name, typeof(T))
        {
            Value = value;
        }

        public ITvProperty Clone()
        {
            return new ImmutableTvProperty<T>(Name, Value);
        }


    }

}
