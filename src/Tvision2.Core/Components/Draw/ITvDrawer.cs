using Tvision2.Core.Render;

namespace Tvision2.Core.Components.Draw
{
    public interface ITvDrawer<T>
    {
        void Draw(RenderContext<T> context);
    }
}
