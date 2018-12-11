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
            return new Viewport(TvPoint.Zero, cols, rows, zindex: 0);
        }

        public IViewport BottomViewport(int vprows = 1)
        {
            var (rows, cols) = _console.GetConsoleWindowSize();
            return new Viewport(new TvPoint(0, rows - vprows), cols, vprows, zindex: 0);
        }
    }
}
