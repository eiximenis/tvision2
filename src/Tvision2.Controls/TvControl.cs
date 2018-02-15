using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Controls.Behavior;
using Tvision2.Core.Components;

namespace Tvision2.Controls
{
    public abstract class TvControl<TState> : ITvControl<TState>
    {
        public IControlData  Data { get; }
        private TvComponent<TState> _component;
        public TState State { get; }

        public TvControl(TState state, IControlData data = null)
        {
            Data = data ?? new TvControlData();
            State = state;
            CreateComponent();
        }

        TvComponent ITvControl.AsComponent() => _component;

        public TvComponent<TState> AsComponent() => _component;

        protected void CreateComponent()
        {
            var cmp = new TvComponent<TState>(Data.Style, State, Data.Name);
            cmp.AddBehavior(new ControlStateBehavior<TState>(Data));
            AddComponentElements(cmp);
            _component = cmp;
        }

        protected abstract void AddComponentElements(TvComponent<TState> cmp);
    }
}
