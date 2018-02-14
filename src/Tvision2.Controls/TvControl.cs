using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Controls.Behavior;
using Tvision2.Core.Components;

namespace Tvision2.Controls
{
    public abstract class TvControl<TState> : ITvControl
        where TState : IControlState
    {
        public TState State { get; }
        private TvComponent<TState> _component;

        public TvControl(TState state)
        {
            State = state;
            CreateComponent();
        }

        TvComponent ITvControl.AsComponent() => _component;

        public TvComponent<TState> AsComponent() => _component;

        protected void CreateComponent()
        {
            var cmp = new TvComponent<TState>(State.Style, State, State.Name);
            cmp.AddBehavior(new ControlStateBehavior<TState>(State));
            AddComponentElements(cmp);
            _component = cmp;
        }

        protected abstract void AddComponentElements(TvComponent<TState> cmp);
    }
}
