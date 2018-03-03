using Tvision2.Core.Components;

namespace Tvision2.Core.Render
{
    public class RenderContext<T>
    {
        public IBoxModel BoxModel { get; }
        public Viewport Viewport { get; }
        public T State { get; }

        private VirtualConsole _console;

        public RenderContext(IBoxModel boxModel, Viewport viewport, VirtualConsole console, T state)
        {
            _console = console;
            viewport.AttachConsole(console);
            BoxModel = boxModel;
            Viewport = viewport;
            State = state;
        }

        public RenderContext<TD> CloneWithNewState<TD>(TD newState) => new RenderContext<TD>(BoxModel, Viewport, _console, newState);
    }
}