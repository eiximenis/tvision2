using Tvision2.Core.Render;
using Tvision2.Engine.Console;

namespace Tvision2.Viewports
{
    internal class ViewportFactory : IViewportFactory
    {
        private readonly IConsoleDriver _console;
        public ViewportFactory(IConsoleDriver console)
        {
            _console = console;
        }

        public IViewport FullViewport()
        {
            return new Viewport(TvPoint.Zero, _console.ConsoleBounds, zindex: 0);
        }

        public IViewport BottomViewport(int vprows = 1)
        {
            var bounds = _console.ConsoleBounds;
            return new Viewport(new TvPoint(0, bounds.Rows - vprows), new TvBounds (vprows, bounds.Cols), zindex: 0);
        }
    }
}
