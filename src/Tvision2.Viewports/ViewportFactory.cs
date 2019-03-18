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
            var (rows, cols) = _console.GetConsoleWindowSize();
            return new Viewport(TvPoint.Zero, new TvBounds(rows, cols), zindex: 0);
        }

        public IViewport BottomViewport(int vprows = 1)
        {
            var (rows, cols) = _console.GetConsoleWindowSize();
            return new Viewport(new TvPoint(0, rows - vprows), new TvBounds (vprows, cols), zindex: 0);
        }
    }
}
