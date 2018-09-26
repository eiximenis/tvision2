using Tvision2.Core.Colors;

namespace Tvision2.Controls.Styles
{
    public interface IStyleBuilder
    {
        IStyleBuilder DesiredStandard(DefaultColorName fore, DefaultColorName back);
        IStyleBuilder DesiredFocused(DefaultColorName fore, DefaultColorName back);
    }
}