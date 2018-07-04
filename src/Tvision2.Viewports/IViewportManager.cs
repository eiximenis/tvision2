using Tvision2.Core.Render;

namespace Tvision2.Viewports
{
    public interface IViewportManager
    {
        void InvalidateViewport(IViewport vieportToInvalidate);
    }
}
