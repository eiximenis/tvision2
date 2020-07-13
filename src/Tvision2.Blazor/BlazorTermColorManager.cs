using Tvision2.Blazor;
using Tvision2.Core.Colors;

namespace Tvision2.ConsoleDriver.BlazorTerm
{
    internal class BlazorTermColorManager : IColorManager
    {
        private readonly BlazorPalette _palette;

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
    }
}