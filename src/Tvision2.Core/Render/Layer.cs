using System;
using System.Collections.Generic;
using System.Runtime;
using System.Runtime.Serialization;
using System.Text;

namespace Tvision2.Core.Render
{

    public enum LayerValues
    {
        Min = int.MinValue,
        Bottom = 0,
        Standard = 1024,
        Top = int.MaxValue,
    }


    public readonly struct Layer
    {

        private readonly int _value;
        private Layer(int value) => _value = value;

        public static Layer FromRaw(short value) => new Layer((int)value);
        public static Layer Standard => new Layer((int)LayerValues.Standard);
        public static Layer Top => new Layer((int)LayerValues.Top);
        public static Layer Min => new Layer((int)LayerValues.Min);
        public static Layer Bottom => new Layer((int)LayerValues.Bottom);

        public Layer MoveToTop(int steps)
        {

            return ((int)LayerValues.Top - steps > _value)
                ? new Layer(_value + steps)
                : Layer.Top;
        }

        public Layer Move(int steps)
        {
            if (steps == 0) return this;
            return steps > 0 ? MoveToTop(steps) : MoveToBottom(-steps);
        }

        public Layer MoveToBottom(int steps)
        {
            return _value - steps > 0
                ? new Layer(_value - steps)
                : Layer.Bottom;
        }

        public static bool operator >(Layer one, Layer two) => one._value > two._value;
        public static bool operator <(Layer one, Layer two) => one._value < two._value;
        public static bool operator ==(Layer one, Layer two) => one._value == two._value;
        public static bool operator !=(Layer one, Layer two) => one._value != two._value;

        public static explicit operator int(Layer layer) => layer._value;

        public override bool Equals(object obj)
        {
            return obj is Layer ? ((Layer)obj)._value == _value : false;
        }

        public override int GetHashCode() => _value.GetHashCode();

    }
}
