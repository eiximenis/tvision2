using System;
using System.Collections.Generic;
using System.Linq;
using Tvision2.Core.Components.Behaviors;
using Tvision2.Core.Components.Draw;
using Tvision2.Core.Engine;
using Tvision2.Core.Render;
using Tvision2.Events;

namespace Tvision2.Core.Components
{


    public abstract class TvComponent
    {

        public IBoxModel BoxModel { get; }
        public bool NeedToRedraw { get; protected set; }
        public string Name { get; }
        public void Invalidate() => NeedToRedraw = true;

        public TvComponent(IBoxModel boxModel, string name)
        {
            Name = string.IsNullOrEmpty(name) ? $"TvComponent-{Guid.NewGuid().ToString()}" : name;
            BoxModel = boxModel;
        }

        protected internal abstract void Update(TvConsoleEvents evts);
        protected internal abstract void Draw(VirtualConsole console);
    }

    public class TvComponent<T> : TvComponent
    {
        private ITvDrawer<T> _drawer;
        private readonly List<BehaviorMetadata<T>> _behaviorsMetadata;
        public T State { get; private set; }

        public bool HasFocus { get; internal set; }

        public void SetState(T newState)
        {
            var oldState = State;
            State = newState;
            NeedToRedraw = Object.ReferenceEquals(oldState, State);
        }


        public TvComponent(IBoxModel boxModel, T initialState, string name = null) : base(boxModel, name)
        {
            NeedToRedraw = false;
            _behaviorsMetadata = new List<BehaviorMetadata<T>>();
            _drawer = null;
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

        public TvComponent<T> UseDrawer(Action<RenderContext<T>> action) => UseDrawer(new ActionDrawer<T>(action));

        public TvComponent<T> UseDrawer(ITvDrawer<T> drawer)
        {
            _drawer = drawer;
            return this;
        }


        protected internal override void Update(TvConsoleEvents evts)
        {
            bool updated = NeedToRedraw;
            foreach (var mdata in _behaviorsMetadata)
            {
                var ctx = new BehaviorContext<T>(State, evts);
                if (mdata.Schedule == BehaviorSchedule.OncePerFrame
                    || (mdata.Schedule == BehaviorSchedule.OnEvents && evts != null))
                {
                    updated = mdata.Behavior.Update(ctx) || updated;
                }
            }

            NeedToRedraw = updated;
        }


        protected internal override void Draw(VirtualConsole console)
        {

            var context = new RenderContext<T>(BoxModel, console, State);
            _drawer.Draw(context);
            NeedToRedraw = false;
        }

    }
}

