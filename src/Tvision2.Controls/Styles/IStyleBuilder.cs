using Tvision2.Core.Colors;

namespace Tvision2.Controls.Styles
{
    public interface IStyleBuilder
    {
        IStyleBuilder DesiredStandard(TvisionColor fore, TvisionColor back,CharacterAttributeModifiers attributes = CharacterAttributeModifiers.Normal);
        IStyleBuilder DesiredFocused(TvisionColor fore, TvisionColor back, CharacterAttributeModifiers attributes = CharacterAttributeModifiers.Normal);
        IStyleBuilder DesiredAlternate(TvisionColor fore, TvisionColor back, CharacterAttributeModifiers attributes = CharacterAttributeModifiers.Normal);
        IStyleBuilder DesiredAlternateFocused(TvisionColor fore, TvisionColor back, CharacterAttributeModifiers attributes = CharacterAttributeModifiers.Normal);
        IStyleBuilder DesiredCustom(string name, TvisionColor fore, TvisionColor back, CharacterAttributeModifiers attributes = CharacterAttributeModifiers.Normal);
    }
}