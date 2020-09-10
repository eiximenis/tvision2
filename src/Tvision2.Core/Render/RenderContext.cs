using System;
using System.Dynamic;
using Tvision2.Core.Colors;
using Tvision2.Core.Components;
using Tvision2.Core.Components.Draw;
using Tvision2.Core.Engine;

namespace Tvision2.Core.Render
{

    public class RenderContextData
    {
        internal VirtualConsole Console {get;}
        internal IViewport Viewport { get; private set; }

        internal ComponentTreeNode Parent { get; }
        internal ComponentTreeNode Node { get; }

        internal RedrawNeededAction RedrawNeededAction { get; }
        
        public RenderContextData(IViewport viewport, VirtualConsole console, ComponentTreeNode node, RedrawNeededAction redrawAction)
        {
            Viewport = viewport;
            Console = console;
            Parent = node.Parent;
            Node = node;
            RedrawNeededAction = redrawAction;
        }

        internal void ApplyDrawResult(DrawResult result)
        {
            if (result != DrawResult.Done)
            {
                Viewport = new Viewport(Viewport.Position + result.Displacement, Viewport.Bounds.Reduce(result.BoundsAdjustement), Viewport.ZIndex, Viewport.Flow);
            }
        }

    }

    public class RenderContext : ICursorContext
    {
        
        public IViewport Viewport { get => _data.Viewport; }
        ICursorContext CursorContext => this;
        protected readonly RenderContextData _data;
        public RedrawNeededAction RedrawNeededAction { get => _data.RedrawNeededAction; }


        public RenderContext(IViewport viewport, VirtualConsole console, ComponentTreeNode node, RedrawNeededAction redrawAction)
        {
            _data = new RenderContextData(viewport, console, node, redrawAction);
        }

        protected RenderContext(RenderContextData data)
        {
            _data = data;
        }

        public void DrawStringAt(string value, TvPoint location, TvColorPair colors)
        {
            if (Viewport.Flow == FlowModel.None)
            {
                ViewportHelperNoneFlow.DrawStringAt(value, location, new CharacterAttribute(colors), Viewport, _data.Console);
            }
            else
            {
                ViewportHelperLineBreakFlow.DrawStringAt(value, location, new CharacterAttribute(colors), Viewport, _data.Console);
            }
        }

        public void DrawStringAt(string value, TvPoint location, CharacterAttribute attr)
        {
            if (Viewport.Flow == FlowModel.None)
            {
                ViewportHelperNoneFlow.DrawStringAt(value, location, attr, Viewport, _data.Console);
            }
            else
            {
                ViewportHelperLineBreakFlow.DrawStringAt(value, location, attr, Viewport, _data.Console);
            }
        }

        public void DrawChars(char value, int count, TvPoint location, TvColorPair colors)
        {
            if (Viewport.Flow == FlowModel.None)
            {
                ViewportHelperNoneFlow.DrawChars(value, count, location, new CharacterAttribute(colors), Viewport, _data.Console);
            }
            else
            {
                ViewportHelperLineBreakFlow.DrawChars(value, count, location, new CharacterAttribute(colors), Viewport, _data.Console);
            }
        }

        public void DrawChars(char value, int count, TvPoint location, CharacterAttribute attribute)
        {
            if (Viewport.Flow == FlowModel.None)
            {
                ViewportHelperNoneFlow.DrawChars(value, count, location, attribute, Viewport, _data.Console);
            }
            else
            {
                ViewportHelperLineBreakFlow.DrawChars(value, count, location, attribute, Viewport, _data.Console);
            }
        }

        public void Clear()
        {
            ViewportHelper.Clear(Viewport, _data.Console);
        }

        public void Fill(CharacterAttribute attr)
        {
            ViewportHelper.Fill(attr, Viewport, _data.Console);
        }

        void ICursorContext.SetCursorAt(int left, int top)
        {
            var point = ViewportHelperNoneFlow.ViewPointToConsolePoint(TvPoint.FromXY(left, top), Viewport.Position);
            _data.Console.Cursor.MoveTo(point.Left, point.Top);
        }

        void ICursorContext.HideCursor()
        {
            _data.Console.Cursor.Hide();
        }

        public TRootState GetRootState<TRootState>() =>
            ((TvComponent<TRootState>)_data.Parent.Root.Data.Component).State;

        public TParentState GetParentState<TParentState>() =>
            ((TvComponent<TParentState>)_data.Parent.Data.Component).State;

        public bool ComponentHasParent => _data.Parent != null;

        public T GetTag<T>() => _data.Node.GetTag<T>();

        internal void ApplyDrawResult(DrawResult result) => _data.ApplyDrawResult(result);

    }

    public class RenderContext<T> : RenderContext
    {       
        public T State { get; }

        public RenderContext(IViewport viewport, VirtualConsole console, ComponentTreeNode node, RedrawNeededAction redrawAction, T state) : 
            base(viewport, console, node, redrawAction)
        {
            State = state;
        }

        private RenderContext(RenderContextData data, T state) : base(data)
        {
            State = state;
        }

        public RenderContext<U> AsRenderContext<U>(Func<T, U> mapper)
        {
            return new RenderContext<U>(_data, mapper(State));
        }
    }
}