using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Tvision2.Core.Components.Behaviors;
using Tvision2.Core.Components.Draw;
using Tvision2.Core.Engine;
using Tvision2.Core.Render;
using Tvision2.Events;

namespace Tvision2.Core.Components
{

    public enum RedrawNeededAction
    {
        None,
        Standard,
        Full
    }

    public enum InvalidateReason
    {
        StateChanged,
        FullDrawRequired
    }


    public abstract class TvComponent
    {
        private readonly TvComponentMetadata _metadata;
        protected Dictionary<Guid, IViewport> _viewports;
        public RedrawNeededAction NeedToRedraw { get; protected set; }
        public string Name { get; }
        public void Invalidate(InvalidateReason reasonToInvalidate)
        {
            NeedToRedraw = reasonToInvalidate == InvalidateReason.StateChanged ? RedrawNeededAction.Standard : RedrawNeededAction.Full;
        }
        public TvComponentMetadata Metadata => _metadata;

        public Guid ComponentId { get; }

        protected readonly List<ITvDrawer> _drawers;
        protected readonly List<IBehaviorMetadata> _behaviorsMetadata;

        public TvComponent(string name, Action<IConfigurableComponentMetadata> configAction = null)
        {
            ComponentId = Guid.NewGuid();
            _metadata = new TvComponentMetadata(this);
            configAction?.Invoke(_metadata);
            _drawers = new List<ITvDrawer>();
            _behaviorsMetadata = new List<IBehaviorMetadata>();
            _viewports = new Dictionary<Guid, IViewport>();
            Name = string.IsNullOrEmpty(name) ? $"TvComponent-{ComponentId}" : name.Replace("<$>", ComponentId.ToString());
        }


        public void RemoveAllBehaviors()
        {
            _behaviorsMetadata.Clear();
        }

        public void RemoveAllDrawers()
        {
            _drawers.Clear();
        }

        public Guid AddViewport(IViewport viewport)
        {
            var key = _viewports.Any() ? Guid.NewGuid() : Guid.Empty;
            _viewports.Add(key, viewport);
            _metadata?.OnViewportChanged(key, null, viewport);
            return key;
        }
        public IViewport Viewport => _viewports.TryGetValue(Guid.Empty, out IViewport value) ? value : null;
        public IViewport GetViewport(Guid guid) => _viewports.TryGetValue(guid, out IViewport value) ? value : null;
        public IEnumerable<KeyValuePair<Guid, IViewport>> Viewports => _viewports;

        public IEnumerable<IBehaviorMetadata> BehaviorsMetadatas => _behaviorsMetadata;

        public void UpdateViewport(IViewport newViewport, bool addIfNotExists = false) => UpdateViewport(Guid.Empty, newViewport, addIfNotExists);

        public void UpdateViewport(Guid guid, IViewport newViewport, bool addIfNotExists = false)
        {
            if (_viewports.TryGetValue(guid, out IViewport oldvp))
            {
                _viewports[guid] = newViewport;
                _metadata?.OnViewportChanged(guid, oldvp, newViewport);
            }
            else if (addIfNotExists)
            {
                _viewports.Add(guid, newViewport);
                _metadata?.OnViewportChanged(guid, null, newViewport);
            }
        }


        public void AddDrawer(ITvDrawer drawer) => _drawers.Add(drawer);

        public void InsertDrawerAt(ITvDrawer drawer, int index)
        {
            _drawers.Insert(0, drawer);
        }

        protected internal abstract void Update(UpdateContext ctx);

        protected abstract void DoDraw(VirtualConsole console, ComponentTreeNode parent);

        protected internal void Draw(VirtualConsole console, ComponentTreeNode parent)
        {
            DoDraw(console, parent);
            NeedToRedraw = RedrawNeededAction.None;
        }

    }

    public sealed class TvComponent<T> : TvComponent
    {
        public T State { get; private set; }

        public void SetState(T newState)
        {
            var oldState = State;
            State = newState;
            NeedToRedraw = Object.ReferenceEquals(oldState, State) ? RedrawNeededAction.None : RedrawNeededAction.Standard;
        }


        public TvComponent(T initialState, string name = null, Action<IConfigurableComponentMetadata> configAction = null) : base(name, configAction)
        {
            NeedToRedraw = RedrawNeededAction.None;
            State = initialState;
        }

        public void AddBehavior(Func<T, bool> action)
        {
            AddBehavior(new StateActionBehavior<T>(action));
        }

        public void AddStateBehavior(Func<T,T> newState)
        {
            AddBehavior(new ChangeStateBhavior<T>(newState, this));
        }

        public void AddBehavior(ITvBehavior<T> behavior, Action<IBehaviorMetadata<T>> metadataAction = null)
        {
            var metadata = new BehaviorMetadata<T>(behavior);
            metadataAction?.Invoke(metadata);
            _behaviorsMetadata.Add(metadata);
        }


        public void AddBehavior<TB>(Action<IFactoryBehaviorMetadata<TB, T>> metadataAction = null)
            where TB : ITvBehavior<T>
        {
            var metadata = new FactoryBehaviorMetadata<TB, T>();
            metadataAction?.Invoke(metadata);
            _behaviorsMetadata.Add(metadata);
        }

        public bool HasBehavior<TBehaviorMetadata>()
        {
            return _behaviorsMetadata.Any(bm => bm is IBehaviorMetadata<TBehaviorMetadata>);
        }

        public TvComponent<T> AddDrawer(Action<RenderContext<T>> action) => AddDrawer(new ActionDrawer<T>(action));

        public TvComponent<T> AddDrawer(ITvDrawer<T> drawer)
        {
            _drawers.Add(drawer);
            return this;
        }

        protected internal override void Update(UpdateContext ctx)
        {
            var updated = NeedToRedraw != RedrawNeededAction.None;
            foreach (var mdata in _behaviorsMetadata)
            {
                var evts = ctx.Events;
                var behaviorCtx = new BehaviorContext<T>(State, evts, Viewport, ctx.ComponentLocator);
                if (mdata.Schedule == BehaviorSchedule.OncePerFrame
                    || (mdata.Schedule == BehaviorSchedule.OnEvents && evts != TvConsoleEvents.Empty))
                {
                    updated =  ((IBehaviorMetadata<T>)mdata).Behavior.Update(behaviorCtx) || updated;
                }
            }
            NeedToRedraw = updated ? RedrawNeededAction.Standard : RedrawNeededAction.None;
        }


        protected override void DoDraw(VirtualConsole console, ComponentTreeNode parent)
        {
            foreach (var vpnk in _viewports)
            {
                var context = new RenderContext<T>(vpnk.Value, console,  parent, NeedToRedraw, State);
                foreach (var drawer in _drawers)
                {
                    drawer?.Draw(context);
                }
            }
        }

    }
}

