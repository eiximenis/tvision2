using System;
using Tvision2.Core.Components;

namespace Tvision2.Core.Render
{

    public class RenderContext
    {
        protected readonly VirtualConsole _console;
        public IViewport Viewport { get; private set; }
        public RenderContext(IViewport viewport, VirtualConsole console)
        {
            _console = console;
            Viewport = viewport;
        }

        public void DrawStringAt(string value, TvPoint location, int pairIdx)
        {
            ViewportHelper.DrawStringAt(value, location, pairIdx, Viewport, _console);
        }

        public void DrawChars(char value, int count, TvPoint location, int pairIdx)
        {
            ViewportHelper.DrawChars(value, count, location, pairIdx, Viewport, _console);
        }

        public void SetCursorAt(int left, int top)
        {
            _console.Cursor.MoveTo(left, top);
        }

        public void Clear()
        {
            ViewportHelper.Clear(Viewport, _console);
        }

        public void Fill(int pairIdx)
        {
            ViewportHelper.Fill(pairIdx, Viewport, _console);
        }

        public void ApplyBoxModel(IViewport newBoxModel)
        {
            Viewport = newBoxModel;
        }
    }

    public class RenderContext<T> : RenderContext
    {       
        public T State { get; }

        public RenderContext(IViewport viewport, VirtualConsole console, T state) : base(viewport, console)
        {

            State = state;
        }

    }
}