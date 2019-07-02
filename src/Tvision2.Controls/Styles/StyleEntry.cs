using Tvision2.Controls.Backgrounds;
using Tvision2.Core.Colors;
using Tvision2.Core.Render;

namespace Tvision2.Controls.Styles
{
    public class StyleEntry
    {
        public TvColor Foreground { get; }
        public IBackgroundProvider Background { get; }
        public CharacterAttributeModifiers Attributes { get; }

        private readonly TvColor? _fixedBackground;



        public StyleEntry(TvColor foreground, IBackgroundProvider background, CharacterAttributeModifiers modifiers)
        {
            Foreground = foreground;
            Background = background;
            Attributes = modifiers;
            if (Background.IsFixedBackgroundColor)
            {
                _fixedBackground = background.GetColorFor(0, 0, TvBounds.Empty);
            }
            else
            {
                _fixedBackground = null;  
            }
        }

        public CharacterAttribute ToCharacterAttribute(TvPoint location, TvBounds bounds)
        {
            var pair = new TvColorPair(Foreground, _fixedBackground.HasValue ? 
                _fixedBackground.Value : 
                Background.GetColorFor(location.Top, location.Left, bounds));

            return new CharacterAttribute(pair, Attributes);
        }
    }
}