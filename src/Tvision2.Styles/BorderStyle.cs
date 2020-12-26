using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Tvision2.Styles
{
    public enum BorderType
    {
        None = 0,
        Space = 1,
        Fill = 2,
        Double = 3,
        Single = 4
    }

    public readonly struct BorderValue
    {
        private readonly int _value;

        public BorderValue(BorderType horizontal, BorderType vertical)
        {
            _value = ((int)horizontal | (int)vertical << 4);
        }

        public BorderValue(BorderType border) : this(border, border) { }

        public static BorderValue None() => new BorderValue(BorderType.None, BorderType.None);
        public static BorderValue Double() => new BorderValue(BorderType.Double, BorderType.Double);
        public static BorderValue Single() => new BorderValue(BorderType.Single, BorderType.Single);

        public static BorderValue FromHorizontalAndVertical(BorderType horizontal, BorderType vertical) => new BorderValue(horizontal, vertical);

        public bool HasVerticalBorder { get => (_value & 0b11110000) != 0; }
        public bool HasHorizontalBorder { get => (_value & 0b00001111) != 0; }


        public void Deconstruct (out BorderType horizontal, out BorderType vertical)
        {
            horizontal = Horizontal;
            vertical = Vertical;
        }


        public BorderType Horizontal
        {
            get => (BorderType)((_value & 0b11110000) >> 4);
        }
        public BorderType Vertical
        {
            get => (BorderType)(_value & 0b00001111);
        }


    }
}
