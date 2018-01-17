using Tvision2.Core.Components;
using Tvision2.Core.Components.Props;
using Tvision2.Core.Styles;

namespace Tvision2.Core.Render
{
    public class RenderContext
    {
        public StyleSheet Style{ get; }
        public Viewport Viewport{ get; }
        public IPropertyBag Props { get; }


        public RenderContext(StyleSheet style, Viewport viewport, VirtualConsole console, IPropertyBag props)
        {
            viewport.AttachConsole(console);
            Style = style;
            Viewport = viewport;
            Props = props;
        }
    }
}