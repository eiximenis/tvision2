using Tvision2.Controls.Styles;
using Tvision2.Core.Components;

namespace Tvision2.Controls
{
    public interface ITvControl
    {
        TvComponent AsComponent();
        string ControlType { get; }
        AppliedStyle Style { get; }

        TvControlMetadata Metadata { get; }

        void OnFocus();
    }

    public interface ITvControl<TState> : ITvControl
    {
        new TvComponent<TState> AsComponent();
    }
}