using System;
using Tvision2.Core.Components;

namespace Tvision2.Core.Render
{
    public class RenderContext<T>
    {
        private readonly VirtualConsole _console;

        public IViewport Viewport { get; private set; }
        public T State { get; }

        public RenderContext(IViewport viewport, VirtualConsole console, T state)
        {
            _console = console;
            Viewport = viewport;
            State = state;
        }

        public RenderContext<TD> CloneWithNewState<TD>(TD newState) => new RenderContext<TD>(Viewport, _console, newState);

        public void DrawStringAt(string value, TvPoint location, ConsoleColor foreColor, ConsoleColor backColor)
        {
            ViewportHelper.DrawStringAt(value, location, foreColor, backColor, Viewport, _console);
        }

        public void DrawChars(char value, int count, TvPoint location, ConsoleColor foreColor, ConsoleColor backColor)
        {
            ViewportHelper.DrawChars(value, count, location, foreColor, backColor, Viewport, _console);
        }

        public void ApplyBoxModel(IViewport newBoxModel)
        {
            Viewport = newBoxModel;
        }


        public void Clear()
        {
            ViewportHelper.Clear(Viewport, _console);
        }

        public void Fill(ConsoleColor backColor)
        {
            ViewportHelper.Fill(backColor, Viewport, _console);
        }
    }
}