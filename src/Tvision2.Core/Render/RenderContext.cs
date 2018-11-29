using System;
using Tvision2.Core.Colors;
using Tvision2.Core.Components;

namespace Tvision2.Core.Render
{

    public class RenderContext : ICursorContext
    {
        protected readonly VirtualConsole _console;
        public IViewport Viewport { get; private set; }
        ICursorContext CursorContext => this;
        public RenderContext(IViewport viewport, VirtualConsole console)
        {
            _console = console;
            Viewport = viewport;
        }

        public void DrawStringAt(string value, TvPoint location, CharacterAttribute attr)
        {
            ViewportHelper.DrawStringAt(value, location, attr, Viewport, _console);
        }

        public void DrawChars(char value, int count, TvPoint location, CharacterAttribute attribute)
        {
            ViewportHelper.DrawChars(value, count, location, attribute, Viewport, _console);
        }

        public void Clear()
        {
            ViewportHelper.Clear(Viewport, _console);
        }

        public void Fill(CharacterAttribute attr)
        {
            ViewportHelper.Fill(attr, Viewport, _console);
        }

        public void ApplyBoxModel(IViewport newBoxModel)
        {
            Viewport = newBoxModel;
        }

        void ICursorContext.SetCursorAt(int left, int top)
        {
            var point = ViewportHelper.ViewPointToConsolePoint(new TvPoint(left, top), Viewport.Position);
            _console.Cursor.MoveTo(point.Left, point.Top);
        }

        void ICursorContext.HideCursor()
        {
            _console.Cursor.Hide();
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