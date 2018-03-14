using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Controls.Behavior;
using Tvision2.Controls.Styles;
using Tvision2.Core.Components;
using Tvision2.Core.Render;
using Tvision2.Events;

namespace Tvision2.Controls
{
    public abstract class TvControl<TState> : ITvControl<TState>
        where TState : IDirtyObject
    {

        private IStyleSheet _currentStyles;
        private TvComponent<TState> _component;
        private TvControlData _controlData;

        public TState State { get; }

        public string ControlType { get; }
        public AppliedStyle Style { get; }

        public TvControl(ISkin skin, IBoxModel boxModel, TState initialState)
        {
            ControlType = GetType().Name.ToLowerInvariant();
            _currentStyles = skin.GetControlStyle(this);
            Style = _currentStyles.BuildStyle(boxModel);
            State = initialState;
            _component = new TvComponent<TState>(Style, initialState);
            _controlData = new TvControlData(Style, initialState);

            AddElements();
        }

        protected void AddElements()
        {
            _component.AddBehavior(new ControlStateBehavior<TState>(_controlData));
            _component.UseDrawer(OnDraw);
            AddCustomElements(_component);
        }


        protected void ApplyNewBoxModel(RenderContext<TState> context, IBoxModel newBoxModel)
        {
            context.Clear();
            context.ApplyBoxModel(newBoxModel);
            context.Fill(Style.BackColor);
        }

        protected abstract void OnDraw(RenderContext<TState> context);

        protected virtual void AddCustomElements(TvComponent<TState> component) { }

        public void OnFocus()
        {
            var pos = Style.Position;
            _component.Metadata.Console.SetCursorAt(pos.Left, pos.Top);
        }

        public TvComponent<TState> AsComponent() => _component;

        TvComponent ITvControl.AsComponent() => _component;
    }
}
