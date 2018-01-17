using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Controls.Behavior;
using Tvision2.Core.Components;
using Tvision2.Core.Components.Props;

namespace Tvision2.Controls
{
    public abstract class TvControl<TState> : ITvControl
        where TState : IControlState
    {
        public TState State { get; }
        private TvComponent _component;

        public TvControl(TState state)
        {
            State = state;
        }

        public TvComponent AsComponent() => _component;

        protected virtual ComponentDefinition CreateDefinition()
        {
            var definition = new ComponentDefinition();
            definition.AddBehavior(new ControlStateBehavior<TState>(State));
            return definition;
        }

        protected void CreateComponent(IComponentDefinition definition)
        {
            var cmp = new TvComponent(ImmutablePropertyBag.FromObject(State), State.Style, State.Name);
            cmp.ApplyDefinition(definition);
            _component = cmp;
        }
    }
}
