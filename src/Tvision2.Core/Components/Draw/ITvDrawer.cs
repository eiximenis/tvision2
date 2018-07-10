using Tvision2.Core.Render;

namespace Tvision2.Core.Components.Draw
{

    public interface ITvDrawer
    {
        void Draw(RenderContext context);
    }

    public interface ITvDrawer<T>  : ITvDrawer
    {
        void Draw(RenderContext<T> context);
    }
}
