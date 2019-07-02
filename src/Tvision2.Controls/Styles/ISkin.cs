using Tvision2.Core.Colors;

namespace Tvision2.Controls.Styles
{
    public interface ISkin
    {
        IStyle this[string name] { get; }
    }
}
