using Tvision2.Blazor;
using Tvision2.Core.Colors;

namespace Tvision2.ConsoleDriver.BlazorTerm
{
    internal class BlazorTermColorManager : IColorManager
    {
        private readonly BlazorPalette _palette;

        private const string SGR_RGB_FORE = "\x1b[38;2;{0};{1};{2}m";                // Truecolor SETAF
        private const string SGR_RGB_BACK = "\x1b[48;2;{0};{1};{2}m";                // Truecolor SETAB


        public CharacterAttribute DefaultAttribute => throw new System.NotImplementedException();

        public IPalette Palette => _palette;

        public BlazorTermColorManager()
        {
            _palette = new BlazorPalette();
        }

        public CharacterAttribute BuildAttributeFor(TvColor fore, TvColor back, CharacterAttributeModifiers attrs = CharacterAttributeModifiers.Normal)
        {
            return new CharacterAttribute(new TvColorPair(fore, back), attrs);
        }

        private (TvColor fore, TvColor back) DecodeColorsFromAttribute(CharacterAttribute attr)
        {
            return (attr.Fore, attr.Back);
        }

        public string GetAttributeSequence(CharacterAttribute attributes)
        {
            var (fore, back) = DecodeColorsFromAttribute(attributes);
            var (fr, fg, fb) = fore.Rgb;
            var forestring = string.Format(SGR_RGB_FORE, fr.ToString(), fg.ToString(), fb.ToString());

            var (br, bg, bb) = back.Rgb;
            var backstring = string.Format(SGR_RGB_BACK, br.ToString(), bg.ToString(), bb.ToString());
            return forestring + backstring;
        }
    }
}