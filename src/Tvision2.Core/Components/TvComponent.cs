using System;
using System.Collections.Generic;
using System.Linq;
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

        private readonly List<BehaviorMetadata> _behaviorsMetadata;
        public StyleSheet Style { get; }

        public bool IsDirty { get; private set; }

        public bool HasFocus { get; internal set; }
        public string Name { get; }

        internal IPropertyBag Properties => _props; 

        public TvComponent(IPropertyBag initialProps, StyleSheet style, string name = null)
        {
            IsDirty = false;
            _behaviorsMetadata = new List<BehaviorMetadata>();
            Name = string.IsNullOrEmpty(name) ? $"TvComponent-{Guid.NewGuid().ToString()}" : name;
            _props = initialProps;
            Style = style ?? new StyleSheet();
            _drawers = new Dictionary<string, ITvDrawer>();
            HasFocus = false;
        }

        public void AddBehavior(ITvBehavior behavior, Action<BehaviorMetadata> metadataAction = null)
        {
            var metadata = new BehaviorMetadata(behavior);
            metadataAction?.Invoke(metadata);
            _behaviorsMetadata.Add(metadata);
        }

        public void AddBehavior(Func<BehaviorContext, IPropertyBag> behaviorFunc, Action<BehaviorMetadata> metadataAction = null) => AddBehavior(new ActionBehavior(behaviorFunc), metadataAction);

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
            foreach (var mdata in _behaviorsMetadata.Where(m => m.Schedule == BehaviorSchedule.OncePerFrame))
            {
                var ctx = new BehaviorContext(props, dispatcher, evts);
                props = mdata.Behavior.Update(ctx) ?? props;
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