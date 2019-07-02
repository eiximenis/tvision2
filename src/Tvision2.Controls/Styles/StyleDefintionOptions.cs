using System;
using Tvision2.Controls.Backgrounds;
using Tvision2.Core.Colors;

namespace Tvision2.Controls.Styles
{
    public class StyleDefintionOptions
    {
        internal TvColor ForeColor { get; private set; }
        internal IBackgroundProvider Background { get; private set; } 

        internal CharacterAttributeModifiers Modifiers { get; private set; }

        public StyleDefintionOptions()
        {
            Modifiers = CharacterAttributeModifiers.Normal;
            ForeColor = TvColor.Black;
            Background = SolidColorBackgroundProvider.BlackBackground;
        }

        public StyleDefintionOptions AddModifier(CharacterAttributeModifiers modifier)
        {
            Modifiers |= modifier;
            return this;
        }

        public StyleDefintionOptions UseForeground(TvColor fore)
        {
            ForeColor = fore;
            return this;
        }

        public StyleDefintionOptions UseBackground(TvColor back)
        {
            Background = new SolidColorBackgroundProvider(back);
            return this;
        }

        public StyleDefintionOptions UseBackground<TB>(Func<TB> builder) where TB : IBackgroundProvider
        {
            Background = builder.Invoke();
            return this;
        }
    }
}