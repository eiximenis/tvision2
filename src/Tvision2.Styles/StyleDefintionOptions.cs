using System;
using Tvision2.Core.Colors;
using Tvision2.Styles.Backgrounds;

namespace Tvision2.Styles
{
    public class StyleDefintionOptions
    {
        internal IColorProvider Foreground { get; private set; }
        internal IColorProvider Background { get; private set; } 

        internal CharacterAttributeModifiers Modifiers { get; private set; }

        public StyleDefintionOptions()
        {
            Modifiers = CharacterAttributeModifiers.Normal;
            Foreground = new SolidColorBackgroundProvider(TvColor.White);
            Background = SolidColorBackgroundProvider.BlackBackground;
        }

        public StyleDefintionOptions AddModifier(CharacterAttributeModifiers modifier)
        {
            Modifiers |= modifier;
            return this;
        }

        public StyleDefintionOptions UseForeground(TvColor fore)
        {
            Foreground = new SolidColorBackgroundProvider(fore);
            return this;
        }

        public StyleDefintionOptions UseBackground(TvColor back)
        {
            Background = new SolidColorBackgroundProvider(back);
            return this;
        }

        public StyleDefintionOptions UseBackground<TB>(Func<TB> builder) where TB : IColorProvider
        {
            Background = builder.Invoke();
            return this;
        }

        public StyleDefintionOptions UseForegound<TF>(Func<TF> builder) where TF : IColorProvider
        {
            Foreground = builder.Invoke();
            return this;
        }
    }
}