using System;
using Tvision2.Controls.Backgrounds;
using Tvision2.Core;
using Tvision2.Core.Colors;

namespace Tvision2.Controls.Styles
{

    public interface IStyleBuilder
    {
        IStyleEntryBuilder Default();
        IStyleEntryBuilder When(Func<ColorMode, bool> colorModePredicate);
        IStyleEntryBuilder When(Func<IPalette, bool> palettePredicate);
    }

    public interface IStyleEntryBuilder
    {
        IStyleEntryBuilder DesiredStandard(Action<StyleDefintionOptions> optionsAction);
        IStyleEntryBuilder DesiredFocused(Action<StyleDefintionOptions> optionsAction);
        IStyleEntryBuilder DesiredAlternate(Action<StyleDefintionOptions> optionsAction);
        IStyleEntryBuilder DesiredAlternateFocused(Action<StyleDefintionOptions> optionsAction);
        IStyleEntryBuilder DesiredCustom(string name, Action<StyleDefintionOptions> optionsAction);

    }

}