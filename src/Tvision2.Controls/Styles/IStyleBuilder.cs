using Tvision2.Core.Colors;

namespace Tvision2.Controls.Styles
{
    public interface IStyleBuilder
    {
        IStyleBuilder DesiredStandard(DefaultColorName fore, DefaultColorName back,CharacterAttributeModifiers attributes = CharacterAttributeModifiers.Normal);
        IStyleBuilder DesiredFocused(DefaultColorName fore, DefaultColorName back, CharacterAttributeModifiers attributes = CharacterAttributeModifiers.Normal);
        IStyleBuilder DesiredAlternate(DefaultColorName fore, DefaultColorName back, CharacterAttributeModifiers attributes = CharacterAttributeModifiers.Normal);
        IStyleBuilder DesiredAlternateFocused(DefaultColorName fore, DefaultColorName back, CharacterAttributeModifiers attributes = CharacterAttributeModifiers.Normal);
        IStyleBuilder DesiredCustom(string name, DefaultColorName fore, DefaultColorName back, CharacterAttributeModifiers attributes = CharacterAttributeModifiers.Normal);
    }
}