using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tvision2.Core.Colors
{
    
    public static class TvColorNames
    {
        private static readonly string[] _names = new[] { "Black", "Red", "Green", "Yellow", "Blue", "Magenta", "Cyan", "White"};
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

        public static IEnumerable<string> AlLStandardColorNames => _names;

        public static string NameOf(int value)
        {
            return _names[(value % _names.Length + 1) - 1];
        }

        public static string NameOf(TvColor color) => NameOf(color.Value);
    }

    public enum TvPalettizedComponentValue : short
    {
        Zero = 0,
        One,
        Two,
        Three,
        Four,
        Five
    }

    public struct TvColor
    {
        public readonly int Value;
        private const int RGB_MARKER = (1 << 31);
        private const int PALETTE_MARKER = (1 << 30);
        public const int ANSI3BIT_MAX_VALUE = 7;
        public const int ANSI4BIT_MAX_VALUE = 15;
        
        private const short PALETTIZED_GRAY_START_IDX = 232;
        private const short PALETTIZED_GRAY_RANGE = 24;

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

        public static explicit operator short(TvColor color) => (short) color.Value;
        public static bool operator ==(TvColor one, TvColor other) => one.Value == other.Value;
        public static bool operator !=(TvColor one, TvColor other) => one.Value != other.Value;

        public static bool operator ==(TvColor one, int other) => one.Value == other;
        public static bool operator !=(TvColor one, int other) => one.Value == other;

        public bool IsRgb => (Value & RGB_MARKER) != 0;

        public bool IsPalettized => (Value & PALETTE_MARKER) != 0;


        public static TvColor FromRaw(int raw) => new TvColor(raw);

        public static TvColor FromRGB(byte red, byte green, byte blue)
        {
            var value = red | (green << 8) | (blue << 16) | RGB_MARKER;
            return new TvColor(value);
        }

        public static TvColor FromPaletteValues(TvPalettizedComponentValue red, TvPalettizedComponentValue green,  TvPalettizedComponentValue blue) =>
            TvColor.FromPaletteIndex((short)(16 + 36 * (short) red + 6 * (short) green + (short) blue));

        public static TvColor FromPaletteGrey(short greyIndex) 
            => FromPaletteIndex((short)(PALETTIZED_GRAY_START_IDX + (greyIndex > PALETTIZED_GRAY_RANGE ? PALETTIZED_GRAY_RANGE : greyIndex)));

        public static TvColor FromPaletteIndex(short index)
        {
            var value = (int) index | PALETTE_MARKER;
            return new TvColor(value);
        }

        public int PaletteIndex => Value & ~PALETTE_MARKER;

        public (byte red, byte green, byte blue) Rgb =>
            ((byte) (Value & 0xff), (byte) (Value >> 8), (byte) (Value >> 16));

        public bool IsBasic => Value <= ANSI3BIT_MAX_VALUE;


        public override bool Equals(object obj)
        {
            if (obj is TvColor) return ((TvColor)obj).Value == Value;
            if (obj is int) return (int)obj == Value;
            return base.Equals(obj);
        }

        public override int GetHashCode() => Value.GetHashCode();

        public override string ToString()
        {
            if (IsRgb)
            {
                var (r, g, b) = Rgb;
                return $"RGB({r},{g},{b}) ({Value})";
            }

            if (IsPalettized)
            {
                return $"PAL({PaletteIndex}) ({Value})";
            }

            if (Value < TvColorNames.StandardColorsCount)
            {
                
                return $"STD({TvColorNames.NameOf(Value)}) ({Value})";
            }

            return $"???({Value})";

        }

        public (int red, int green, int blue) Diff(TvColor other)
        {
            var rgb = this.Rgb;
            var otherRgb = other.Rgb;
            return (rgb.red - otherRgb.red, rgb.green - otherRgb.green, rgb.blue - otherRgb.blue);
        }
    }
}
