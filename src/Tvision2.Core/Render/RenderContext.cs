using System;
using Tvision2.Core.Colors;
using Tvision2.Core.Components;
using Tvision2.Core.Engine;

namespace Tvision2.Core.Render
{

    public class RenderContext : ICursorContext
    {
        protected readonly VirtualConsole _console;
        public IViewport Viewport { get; private set; }
        ICursorContext CursorContext => this;
        private readonly ComponentTreeNode _parent;


        public RenderContext(IViewport viewport, VirtualConsole console, ComponentTreeNode parent)
        {
            _console = console;
            Viewport = viewport;
            _parent = parent;
        }

        public void DrawStringAt(string value, TvPoint location, TvColorPair colors)
        {
            ViewportHelper.DrawStringAt(value, location, new CharacterAttribute(colors), Viewport, _console);
        }

        public void DrawStringAt(string value, TvPoint location, CharacterAttribute attr)
        {
            ViewportHelper.DrawStringAt(value, location, attr, Viewport, _console);
        }

        public void DrawChars(char value, int count, TvPoint location, TvColorPair colors)
        {
            ViewportHelper.DrawChars(value, count, location, new CharacterAttribute(colors), Viewport, _console);
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
            var point = ViewportHelper.ViewPointToConsolePoint(TvPoint.FromXY(left, top), Viewport.Position);
            _console.Cursor.MoveTo(point.Left, point.Top);
        }

        void ICursorContext.HideCursor()
        {
            _console.Cursor.Hide();
        }

        public TRootState GetRootState<TRootState>() =>
            ((TvComponent<TRootState>)_parent.Root.Data.Component).State;

        public TParentState GetParentState<TParentState>() =>
            ((TvComponent<TParentState>)_parent.Data.Component).State;

        public bool ComponentHasParent => _parent != null;

    }

    public class RenderContext<T> : RenderContext
    {       
        public T State { get; }

        public RenderContext(IViewport viewport, VirtualConsole console, ComponentTreeNode parent, T state) : 
            base(viewport, console, parent)
        {
            State = state;
        }

    }
}