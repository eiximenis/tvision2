using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Core.Colors
{

    public static class TvColorNames
    {
        public const int Black = 0;
        public const int Red = 1;
        public const int Green = 2;
        public const int Yellow = 3;
        public const int Blue = 4;
        public const int Magenta = 5;
        public const int Cyan = 6;
        public const int White = 7;

        public static IEnumerable<int> AllStandardColors() => new[] { Black, Red, Green, Yellow, Blue, Magenta, Cyan, White };
        public const int StandardColorsCount = 8;
    }

    public struct TvColor
    {
        public readonly int Value;

        public TvColor(int value) => Value = value;

        public TvColor Plus(int valueToAdd) => new TvColor(Value + valueToAdd);

        public readonly static TvColor Black = new TvColor(TvColorNames.Black);
        public readonly static TvColor Red = new TvColor(TvColorNames.Red);
        public static TvColor Green => new TvColor(TvColorNames.Green);
        public static TvColor Yellow => new TvColor(TvColorNames.Yellow);
        public static TvColor Blue => new TvColor(TvColorNames.Blue);
        public static TvColor Magenta => new TvColor(TvColorNames.Magenta);
        public static TvColor Cyan => new TvColor(TvColorNames.Cyan);
        public static TvColor White => new TvColor(TvColorNames.White);

        public static explicit operator short(TvColor color) => (short)color.Value;
        public static bool operator ==(TvColor one, TvColor other) => one.Value == other.Value;
        public static bool operator !=(TvColor one, TvColor other) => one.Value != other.Value;

        public static bool operator ==(TvColor one, int other) => one.Value == other;
        public static bool operator !=(TvColor one, int other) => one.Value == other;


        public override bool Equals(object obj)
        {
            if (obj is TvColor) return ((TvColor)obj).Value == Value;
            if (obj is int) return (int)obj == Value;
            return base.Equals(obj);
        }

        public override int GetHashCode() => Value.GetHashCode();



    }
}
