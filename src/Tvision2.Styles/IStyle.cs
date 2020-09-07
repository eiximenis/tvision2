using Tvision2.Core.Colors;

namespace Tvision2.Styles
{
    public interface IStyle
    {
        StyleEntry Standard { get; }
        StyleEntry Active { get; }
        StyleEntry Alternate { get; }
        StyleEntry AlternateActive { get; }
        StyleEntry this[string name] { get; }
    }
}
