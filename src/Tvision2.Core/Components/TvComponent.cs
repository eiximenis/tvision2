using System;
using System.Collections.Generic;
using System.Linq;
using Tvision2.Core.Components.Behaviors;
using Tvision2.Core.Components.Draw;
using Tvision2.Core.Engine;
using Tvision2.Core.Render;
using Tvision2.Core.Styles;
using Tvision2.Events;

namespace Tvision2.Core.Components
{


    public abstract class TvComponent
    {
        public StyleSheet Style { get; }


        public bool IsDirty { get; protected set; }
        public string Name { get; }


        public TvComponent(StyleSheet style, string name)
        {
            Name = string.IsNullOrEmpty(name) ? $"TvComponent-{Guid.NewGuid().ToString()}" : name;
            Style = style ?? new StyleSheet();
        }

        protected internal abstract void Update(TvConsoleEvents evts);
        protected internal abstract void Draw(Viewport viewport, VirtualConsole console);
    }

    public class TvComponent<T> : TvComponent
    {
        private Dictionary<string, ITvDrawer<T>> _drawers;
        private readonly List<BehaviorMetadata<T>> _behaviorsMetadata;
        public T State { get; private set; }

        public bool HasFocus { get; internal set; }

        public void SetState(T newState)
        {
            var oldState = State;
            State = newState;
            IsDirty = Object.ReferenceEquals(oldState, State);
        }


        public TvComponent(StyleSheet style, T initialState, string name = null) : base(style, name)
        {
            IsDirty = false;
            _behaviorsMetadata = new List<BehaviorMetadata<T>>();
            _drawers = new Dictionary<string, ITvDrawer<T>>();
            HasFocus = false;
            State = initialState;
        }

        public void AddBehavior(ITvBehavior<T> behavior, Action<BehaviorMetadata<T>> metadataAction = null)
        {
            var metadata = new BehaviorMetadata<T>(behavior);
            metadataAction?.Invoke(metadata);
            _behaviorsMetadata.Add(metadata);
        }

        public void AddBehavior(Func<BehaviorContext<T>, bool> behaviorFunc, Action<BehaviorMetadata<T>> metadataAction = null) => AddBehavior(new ActionBehavior<T>(behaviorFunc), metadataAction);

        public TvComponent<T> AddDrawer(Action<RenderContext<T>> action, string name = null) => AddDrawer(new ActionDrawer<T>(action), name);

        public TvComponent<T> AddDrawer(ITvDrawer<T> drawer, string name = null)
        {
            var drawerName = string.IsNullOrEmpty(name) ? $"{drawer.GetType().Name}-{Guid.NewGuid().ToString()}" : name;
            _drawers.Add(drawerName, drawer);
            return this;
        }


        protected internal override void Update(TvConsoleEvents evts)
        {
            bool updated = Style.IsDirty;
            foreach (var mdata in _behaviorsMetadata)
            {
                var ctx = new BehaviorContext<T>(State, evts);
                if (mdata.Schedule == BehaviorSchedule.OncePerFrame
                    || (mdata.Schedule == BehaviorSchedule.OnEvents && evts != null))
                {
                    updated = mdata.Behavior.Update(ctx) || updated;
                }
            }

            IsDirty = updated;
        }

        protected internal override void Draw(Viewport viewport, VirtualConsole console)
        {
            var context = new RenderContext<T>(Style, viewport, console, State);
            foreach (var drawer in _drawers.Values)
            {
                drawer.Draw(context);
            }
            IsDirty = false;
            Style.IsDirty = false;
        }
    }
}

