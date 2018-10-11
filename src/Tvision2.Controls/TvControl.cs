using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tvision2.Controls.Behavior;
using Tvision2.Controls.Styles;
using Tvision2.Core.Components;
using Tvision2.Core.Components.Behaviors;
using Tvision2.Core.Render;
using Tvision2.Events;

namespace Tvision2.Controls
{
    public abstract class TvControl<TState> : ITvControl<TState>
        where TState : IDirtyObject
    {

        public IStyle CurrentStyle { get; }
        private TvComponent<TState> _component;
        public TvControlMetadata Metadata { get; }
        public TState State { get; }
        public string ControlType { get; }
        public IViewport Viewport => _component.Viewport;
        private bool _setFocusPending;

        public TvControl(ISkin skin, IViewport viewport, TState initialState, string name = null)
        {
            _component = new TvComponent<TState>(initialState, name ?? $"TvControl_<$>");
            Metadata = new TvControlMetadata(this, _component.ComponentId);
            var typename = GetType().Name.ToLowerInvariant();
            var genericIdx = typename.IndexOf('`');
            ControlType = genericIdx != -1 ? typename.Substring(0, genericIdx) : typename;
            CurrentStyle = skin.GetControlStyle(this);
            State = initialState;
            _component.AddViewport(viewport);
            _setFocusPending = false;
            AddElements();
        }

        private void AddElements()
        {
            _component.AddBehavior(new ControlStateBehavior<TState>(Metadata));
            _component.AddDrawer(OnDraw);
            foreach (var behavior in GetEventedBehaviors())
            {
                _component.AddBehavior(new FocusControlBeheavior<TState>(Metadata, behavior), opt => opt.UseScheduler(BehaviorSchedule.OnEvents));
            }
            AddCustomElements(_component);
        }


        protected virtual IEnumerable<ITvBehavior<TState>> GetEventedBehaviors()
        {
            return Enumerable.Empty<ITvBehavior<TState>>();
        }


        private void BeginOnDraw(RenderContext<TState> context)
        {
            if (_setFocusPending)
            {
                var pos = CalculateFocusOffset();
                context.SetCursorAt(pos.Left, pos.Top);
                _setFocusPending = false;
            }
        }

        protected abstract void OnDraw(RenderContext<TState> context);

        protected virtual void AddCustomElements(TvComponent<TState> component) { }

        protected virtual TvPoint CalculateFocusOffset() => TvPoint.Zero;

        public void OnFocus()
        {
            var pos = Viewport.Position + CalculateFocusOffset();
            _setFocusPending = true;
        }

        public TvComponent<TState> AsComponent() => _component;

        TvComponent ITvControl.AsComponent() => _component;
    }
}
