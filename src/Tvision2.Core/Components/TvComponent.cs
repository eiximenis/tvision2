using System;
using System.Collections.Generic;
using Tvision2.Core.Components.Behaviors;
using Tvision2.Core.Components.Draw;
using Tvision2.Core.Components.Props;
using Tvision2.Core.Engine;
using Tvision2.Core.Render;
using Tvision2.Core.Styles;

namespace Tvision2.Core.Components
{
    public class TvComponent
    {

        private Dictionary<string, ITvDrawer> _drawers;
        private IPropertyBag _props;

        private readonly List<ITvBehavior> _behaviors;
        public StyleSheet Style { get; }

        public bool IsDirty { get; private set; }

        public bool HasFocus { get; internal set; }
        public string Name { get; }

        internal IPropertyBag Properties => _props;

        public TvComponent(IPropertyBag initialProps, StyleSheet style, string name = null)
        {
            IsDirty = false;
            _behaviors = new List<ITvBehavior>();
            Name = string.IsNullOrEmpty(name) ? $"TvComponent-{Guid.NewGuid().ToString()}" : name;
            _props = initialProps;
            Style = style ?? new StyleSheet();
            _drawers = new Dictionary<string, ITvDrawer>();
            HasFocus = false;
        }

        public void AddBehavior(ITvBehavior behavior)
        {
            _behaviors.Add(behavior);
        }

        public TvComponent AddDrawer(Action<RenderContext> action, string name = null) => AddDrawer(new ActionDrawer(action), name);

        public TvComponent AddDrawer(ITvDrawer drawer, string name = null)
        {
            var drawerName = string.IsNullOrEmpty(name) ? $"{drawer.GetType().Name}-{Guid.NewGuid().ToString()}" : name;
            _drawers.Add(drawerName, drawer);
            return this;
        }


        internal void Update(ITvDispatcher dispatcher, TvEventsCollection evts)
        {
            var props = _props;
            foreach (var behavior in _behaviors)
            {
                var ctx = new BehaviorContext(props, dispatcher, evts);
                props = behavior.Update(ctx) ?? props;
            }

            IsDirty = !_props.IsEqualTo(props);
            _props = props;
        }

        internal void Draw(Viewport viewport, VirtualConsole console)
        {
            var context = new RenderContext(Style, viewport,console,  _props);

            foreach (var drawer in _drawers.Values)
            {
                drawer.Draw(context);
            }
        }
    }
}