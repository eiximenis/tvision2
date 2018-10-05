using Tvision2.Core.Colors;

namespace Tvision2.Controls.Styles
{
    public interface IStyle
    {
        CharacterAttribute Standard { get; }
        CharacterAttribute Focused { get; }
        CharacterAttribute Alternate { get; }
        CharacterAttribute AlternateFocused { get; }
        CharacterAttribute this[string name] { get; }
    }
}
