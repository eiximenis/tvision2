using Tvision2.Core.Render;

namespace Tvision2.Core.Components.Draw
{

    public interface ITvDrawer
    {
        DrawResult Draw(RenderContext context);
    }

    public interface ITvDrawer<T>  : ITvDrawer
    {
        DrawResult Draw(RenderContext<T> context);
    }
}
