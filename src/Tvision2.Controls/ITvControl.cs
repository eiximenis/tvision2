using Tvision2.Core.Components;

namespace Tvision2.Controls
{
    public interface ITvControl
    {
        TvComponent AsComponent();
        string ControlType { get; }
        string Name { get; }
        TvControlMetadata Metadata { get; }
    }

    public interface ITvControl<TState> : ITvControl
    {
        new TvComponent<TState> AsComponent();
    }
}