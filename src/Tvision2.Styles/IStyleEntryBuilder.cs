using System;
using Tvision2.Core;
using Tvision2.Core.Colors;

namespace Tvision2.Styles
{

    public interface IStyleBuilder
    {
        IStyleEntryBuilder Default();
        IStyleEntryBuilder When(Func<ColorMode, bool> colorModePredicate);
        IStyleEntryBuilder When(Func<IPalette, bool> palettePredicate);
    }

    public interface IStyleEntryBuilder
    {
        IStyleAdditionalEntriesBuilder DesiredStandard(Action<StyleDefintionOptions> optionsAction);
    }

    public interface IStyleAdditionalEntriesBuilder
    {
        IStyleAdditionalEntriesBuilder DesiredFocused(Action<StyleDefintionOptions> optionsAction);
        IStyleAdditionalEntriesBuilder DesiredAlternate(Action<StyleDefintionOptions> optionsAction);
        IStyleAdditionalEntriesBuilder DesiredAlternateFocused(Action<StyleDefintionOptions> optionsAction);
        IStyleAdditionalEntriesBuilder Desired(string name, Action<StyleDefintionOptions> optionsAction);
    }

    public interface IPartialStyleEntryBuilder
    {
        IPartialStyleEntryBuilder DesiredStandard(Action<StyleDefintionOptions> optionsAction);
        IPartialStyleEntryBuilder DesiredFocused(Action<StyleDefintionOptions> optionsAction);
        IPartialStyleEntryBuilder DesiredAlternate(Action<StyleDefintionOptions> optionsAction);
        IPartialStyleEntryBuilder DesiredAlternateFocused(Action<StyleDefintionOptions> optionsAction);
        IPartialStyleEntryBuilder Desired(string name, Action<StyleDefintionOptions> optionsAction);
    }

}