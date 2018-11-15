using Tvision2.Core.Colors;

namespace Tvision2.Controls.Styles
{
    public interface IStyleBuilder
    {
        IStyleBuilder DesiredStandard(TvColor fore, TvColor back,CharacterAttributeModifiers attributes = CharacterAttributeModifiers.Normal);
        IStyleBuilder DesiredFocused(TvColor fore, TvColor back, CharacterAttributeModifiers attributes = CharacterAttributeModifiers.Normal);
        IStyleBuilder DesiredAlternate(TvColor fore, TvColor back, CharacterAttributeModifiers attributes = CharacterAttributeModifiers.Normal);
        IStyleBuilder DesiredAlternateFocused(TvColor fore, TvColor back, CharacterAttributeModifiers attributes = CharacterAttributeModifiers.Normal);
        IStyleBuilder DesiredCustom(string name, TvColor fore, TvColor back, CharacterAttributeModifiers attributes = CharacterAttributeModifiers.Normal);
    }
}