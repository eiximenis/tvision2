using Tvision2.Core.Components;
using Tvision2.Core.Styles;

namespace Tvision2.Core.Render
{
    public class RenderContext<T>
    {
        public AppliedStyle Style { get; }
        public Viewport Viewport { get; }
        public T State { get; }

        private VirtualConsole _console;

        public RenderContext(AppliedStyle style, Viewport viewport, VirtualConsole console, T state)
        {
            _console = console;
            viewport.AttachConsole(console);
            Style = style;
            Viewport = viewport;
            State = state;
        }

        public RenderContext<TD> CloneWithNewState<TD>(TD newState) => new RenderContext<TD>(Style, Viewport, _console, newState);
    }
}