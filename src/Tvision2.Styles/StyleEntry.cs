using System;
using Tvision2.Core.Colors;
using Tvision2.Core.Render;
using Tvision2.Styles.Backgrounds;

namespace Tvision2.Styles
{
    public class StyleEntry
    {
        public TvColor Foreground { get; private set; }
        public IBackgroundProvider Background { get; private set; }
        public CharacterAttributeModifiers Attributes { get; private set; }

        private TvColor? _fixedBackground;



        public StyleEntry(TvColor foreground, IBackgroundProvider background, CharacterAttributeModifiers modifiers)
        {
            Foreground = foreground;
            Background = background;
            Attributes = modifiers;

            EvaluateIfBackgroundIsFixedColor();
        }

        private void EvaluateIfBackgroundIsFixedColor()
        {
            _fixedBackground = null;

            if (Background?.IsFixedBackgroundColor == true)
            {
                _fixedBackground = Background.GetColorFor(0, 0, TvBounds.Empty);
            }

        }

        public CharacterAttribute ToCharacterAttribute(TvPoint location, TvBounds bounds)
        {
            var pair = new TvColorPair(Foreground, _fixedBackground.HasValue ?
                _fixedBackground.Value :
                Background.GetColorFor(location.Top, location.Left, bounds));

            return new CharacterAttribute(pair, Attributes);
        }

        internal void Mix(StyleEntry other)
        {
            if (other == null)
            {
                return;
            }

            Foreground = other.Foreground;
            if (other.Background != null)
            {
                Background = other.Background;
                EvaluateIfBackgroundIsFixedColor();
            }
            Attributes |= other.Attributes;

        }
    }
}