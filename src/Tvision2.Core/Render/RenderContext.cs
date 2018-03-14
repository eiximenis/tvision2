using System;
using Tvision2.Core.Components;

namespace Tvision2.Core.Render
{
    public class RenderContext<T>
    {
        private readonly VirtualConsole _console;

        public IBoxModel BoxModel { get; private set; }
        public T State { get; }

        public RenderContext(IBoxModel boxModel, VirtualConsole console, T state)
        {
            _console = console;
            BoxModel = boxModel;
            State = state;
        }

        public RenderContext<TD> CloneWithNewState<TD>(TD newState) => new RenderContext<TD>(BoxModel, _console, newState);

        public void DrawStringAt(string value, TvPoint location, ConsoleColor foreColor, ConsoleColor backColor)
        {
            Viewport.DrawStringAt(value, location, foreColor, backColor, BoxModel, _console);
        }

        public void DrawChars(char value, int count, TvPoint location, ConsoleColor foreColor, ConsoleColor backColor)
        {
            Viewport.DrawChars(value, count, location, foreColor, backColor, BoxModel, _console);
        }

        public void ApplyBoxModel(IBoxModel newBoxModel)
        {
            BoxModel = newBoxModel;
        }


        public void Clear()
        {
            Viewport.Clear(BoxModel, _console);
        }

        public void Fill(ConsoleColor backColor)
        {
            Viewport.Fill(backColor, BoxModel, _console);
        }
    }
}