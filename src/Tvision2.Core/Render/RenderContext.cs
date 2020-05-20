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

        public RedrawNeededAction RedrawNeededAction { get; }


        public RenderContext(IViewport viewport, VirtualConsole console, ComponentTreeNode parent, RedrawNeededAction redrawAction)
        {
            _console = console;
            Viewport = viewport;
            _parent = parent;
            RedrawNeededAction = redrawAction;
        }

        public void DrawStringAt(string value, TvPoint location, TvColorPair colors)
        {
            if (Viewport.Flow == FlowModel.None)
            {
                ViewportHelperNoneFlow.DrawStringAt(value, location, new CharacterAttribute(colors), Viewport, _console);
            }
            else
            {
                ViewportHelperLineBreakFlow.DrawStringAt(value, location, new CharacterAttribute(colors), Viewport, _console);
            }
        }

        public void DrawStringAt(string value, TvPoint location, CharacterAttribute attr)
        {
            if (Viewport.Flow == FlowModel.None)
            {
                ViewportHelperNoneFlow.DrawStringAt(value, location, attr, Viewport, _console);
            }
            else
            {
                ViewportHelperLineBreakFlow.DrawStringAt(value, location, attr, Viewport, _console);
            }
        }

        public void DrawChars(char value, int count, TvPoint location, TvColorPair colors)
        {
            if (Viewport.Flow == FlowModel.None)
            {
                ViewportHelperNoneFlow.DrawChars(value, count, location, new CharacterAttribute(colors), Viewport, _console);
            }
            else
            {
                ViewportHelperLineBreakFlow.DrawChars(value, count, location, new CharacterAttribute(colors), Viewport, _console);
            }
        }

        public void DrawChars(char value, int count, TvPoint location, CharacterAttribute attribute)
        {
            if (Viewport.Flow == FlowModel.None)
            {
                ViewportHelperNoneFlow.DrawChars(value, count, location, attribute, Viewport, _console);
            }
            else
            {
                ViewportHelperLineBreakFlow.DrawChars(value, count, location, attribute, Viewport, _console);
            }
        }

        public void Clear()
        {
            ViewportHelper.Clear(Viewport, _console);
        }

        public void Fill(CharacterAttribute attr)
        {
            ViewportHelper.Fill(attr, Viewport, _console);
        }

        void ICursorContext.SetCursorAt(int left, int top)
        {
            var point = ViewportHelperNoneFlow.ViewPointToConsolePoint(TvPoint.FromXY(left, top), Viewport.Position);
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

        public RenderContext(IViewport viewport, VirtualConsole console, ComponentTreeNode parent, RedrawNeededAction redrawAction, T state) : 
            base(viewport, console, parent, redrawAction)
        {
            State = state;
        }

    }
}