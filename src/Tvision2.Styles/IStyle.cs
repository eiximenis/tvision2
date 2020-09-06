using Tvision2.Core.Colors;

namespace Tvision2.Styles
{
    public interface IStyle
    {
        StyleEntry Standard { get; }
        StyleEntry Focused { get; }
        StyleEntry Alternate { get; }
        StyleEntry AlternateFocused { get; }
        StyleEntry this[string name] { get; }
    }
}
