using Tvision2.Core.Components;
using Tvision2.Core.Styles;

namespace Tvision2.Core.Render
{
    public class RenderContext<T>
    {
        public StyleSheet Style{ get; }
        public Viewport Viewport{ get; }
        public T State { get; }

        public RenderContext(StyleSheet style, Viewport viewport, VirtualConsole console, T state)
        {
            viewport.AttachConsole(console);
            Style = style;
            Viewport = viewport;
            State = state;
        }
    }
}