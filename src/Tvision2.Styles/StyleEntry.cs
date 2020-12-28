using System;
using System.Net;
using Tvision2.Core.Colors;
using Tvision2.Core.Render;
using Tvision2.Styles.Backgrounds;

namespace Tvision2.Styles
{
    public class StyleEntry
    {
        private IColorProvider ForegroundProvider { get; set; }
        private IColorProvider BackgroundProvider { get; set; }
        public CharacterAttributeModifiers Attributes { get; private set; }

        private TvColor? _fixedBackground;
        private TvColor? _fixedForeground;

        public bool IsBackgroundFixed { get => _fixedBackground.HasValue; }
        public bool IsForegroundFixed { get => _fixedForeground.HasValue;  }

        public StyleEntry(IColorProvider foreground, IColorProvider background, CharacterAttributeModifiers modifiers)
        {
            ForegroundProvider = foreground;
            BackgroundProvider = background;
            Attributes = modifiers;
            EvaluateProvidersAreFixedColor();
        }

        private void EvaluateProvidersAreFixedColor()
        {
            if (ForegroundProvider?.IsFixedColor == true)
            {
                _fixedForeground = ForegroundProvider.GetColorFor(0, 0, TvBounds.Empty);
            }

            if (BackgroundProvider?.IsFixedColor == true)
            {
                _fixedBackground = BackgroundProvider.GetColorFor(0, 0, TvBounds.Empty);
            }

        }

        public CharacterAttribute ToCharacterAttribute(TvPoint location, TvBounds bounds)
        {
            var fore = _fixedForeground.HasValue ? _fixedForeground.Value : ForegroundProvider.GetColorFor(location.Top, location.Left, bounds);
            var back = _fixedBackground.HasValue ? _fixedBackground.Value : BackgroundProvider.GetColorFor(location.Top, location.Left, bounds);
            var pair = new TvColorPair(fore,back);
            return new CharacterAttribute(pair, Attributes);
        }

        internal void Mix(StyleEntry other)
        {
            if (other == null)
            {
                return;
            }

            if (other.ForegroundProvider != null)
            {
                ForegroundProvider = other.ForegroundProvider;
            }
            if (other.BackgroundProvider != null)
            {
                BackgroundProvider = other.BackgroundProvider;
            }
            EvaluateProvidersAreFixedColor();
            Attributes |= other.Attributes;

        }
    }
}