using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tvision2.Core.Colors
{

    public static class TvColorNames
    {
        private static string[] _names = new[] { "Black", "Red", "Green", "Yellow", "Blue", "Magenta", "Cyan", "White"};
        public const int Black = 0;
        public const int Red = 1;
        public const int Green = 2;
        public const int Yellow = 3;
        public const int Blue = 4;
        public const int Magenta = 5;
        public const int Cyan = 6;
        public const int White = 7;

        public static IEnumerable<int> AllStandardColors() =>
            new[] {Black, Red, Green, Yellow, Blue, Magenta, Cyan, White};
        public const int StandardColorsCount = 8;

        public static string NameOf(int value)
        {
            return _names[(value % _names.Length + 1) - 1];
        }

        public static string NameOf(TvColor color) => NameOf(color.Value);
    }

    public struct TvColor
    {
        public readonly int Value;
        private const int RGB_MARKER = (1 << 31);

        public TvColor(int value) => Value = value;

        public TvColor Plus(int valueToAdd) => new TvColor(Value + valueToAdd);

        public static readonly TvColor Black = new TvColor(TvColorNames.Black);
        public static readonly TvColor Red = new TvColor(TvColorNames.Red);
        public static readonly TvColor Green = new TvColor(TvColorNames.Green);
        public static readonly TvColor Yellow = new TvColor(TvColorNames.Yellow);
        public static readonly TvColor Blue = new TvColor(TvColorNames.Blue);
        public static readonly TvColor Magenta = new TvColor(TvColorNames.Magenta);
        public static readonly TvColor Cyan = new TvColor(TvColorNames.Cyan);
        public static readonly TvColor White = new TvColor(TvColorNames.White);

        public static explicit operator short(TvColor color) => (short)color.Value;
        public static bool operator ==(TvColor one, TvColor other) => one.Value == other.Value;
        public static bool operator !=(TvColor one, TvColor other) => one.Value != other.Value;

        public static bool operator ==(TvColor one, int other) => one.Value == other;
        public static bool operator !=(TvColor one, int other) => one.Value == other;

        public bool IsRgb => (Value & RGB_MARKER) != 0;
        
        public static TvColor FromRaw(int raw) => new TvColor(raw);
        
        public static TvColor FromRGB(byte red, byte green, byte blue)
        {
            var value = red | (green << 8) | (blue << 16) | (1 << 31);
            return new TvColor(value);
        }

        public (byte red, byte green, byte blue) Rgb =>
            ((byte) (Value & 0xff), (byte) (Value >> 8), (byte) (Value >> 16));


        public override bool Equals(object obj)
        {
            if (obj is TvColor) return ((TvColor)obj).Value == Value;
            if (obj is int) return (int)obj == Value;
            return base.Equals(obj);
        }

        public override int GetHashCode() => Value.GetHashCode();

        public override string ToString()
        {
            var colvalue = Value % TvColorNames.StandardColorsCount;
            var colname = TvColorNames.NameOf(colvalue);
            var diff = Value - (Value % TvColorNames.StandardColorsCount);
            var msg = diff > 0 ? $"+ {diff}" : "";
            return $"{Value} (TvColor.{colname} {msg})";
        }
    }
}
