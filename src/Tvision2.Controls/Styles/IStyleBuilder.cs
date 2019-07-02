using System;
using Tvision2.Controls.Backgrounds;
using Tvision2.Core.Colors;

namespace Tvision2.Controls.Styles
{
    public interface IStyleBuilder
    {
        IStyleBuilder DesiredStandard(Action<StyleDefintionOptions> optionsAction);
        IStyleBuilder DesiredFocused(Action<StyleDefintionOptions> optionsAction);
        IStyleBuilder DesiredAlternate(Action<StyleDefintionOptions> optionsAction);
        IStyleBuilder DesiredAlternateFocused(Action<StyleDefintionOptions> optionsAction);
        IStyleBuilder DesiredCustom(string name, Action<StyleDefintionOptions> optionsAction);
    }

}