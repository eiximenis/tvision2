using Tvision2.Core.Colors;

namespace Tvision2.Styles
{
    public interface ISkin
    {
        IStyle this[string name] { get; }
    }
}
