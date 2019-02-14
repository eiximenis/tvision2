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

    public enum RedrawNeededAction
    {
        None,
        Standard
    }


    public abstract class TvComponent
    {
        private readonly TvComponentMetadata _metadata;
        private bool _mustBeCleared;
        protected Dictionary<Guid, IViewport> _viewports;
        public RedrawNeededAction NeedToRedraw { get; protected set; }
        public string Name { get; }
        public void Invalidate() => NeedToRedraw = RedrawNeededAction.Standard;
        public IComponentMetadata Metadata => _metadata;

        public Guid ComponentId { get; }

        protected readonly List<ITvDrawer> _drawers;
        protected readonly List<IBehaviorMetadata> _behaviorsMetadata;

        public TvComponent(string name, Action<IConfigurableComponentMetadata> configAction = null)
        {
            _mustBeCleared = false;
            ComponentId = Guid.NewGuid();
            _metadata = new TvComponentMetadata(this);
            configAction?.Invoke(_metadata);
            _drawers = new List<ITvDrawer>();
            _behaviorsMetadata = new List<IBehaviorMetadata>();
            _viewports = new Dictionary<Guid, IViewport>();
            Name = string.IsNullOrEmpty(name) ? $"TvComponent-{ComponentId}" : name.Replace("<$>", ComponentId.ToString());
        }

        public void Clear()
        {
            _mustBeCleared = true;
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

        internal void MountedTo(IComponentTree owner)
        {
            _metadata.OnComponentMounted.Invoke(new ComponentMoutingContext(owner,this));
        }
        internal void UnmountedFrom(IComponentTree owner)
        {
            _metadata.OnComponentUnmounted.Invoke(new ComponentMoutingContext(owner, this));
        }

        protected internal abstract void Update(TvConsoleEvents evts);

        protected abstract void DoDraw(VirtualConsole console);

        protected internal void Draw(VirtualConsole console)
        {
            if (_mustBeCleared)
            {
                foreach (var vpkv in _viewports)
                {
                    console.Clear(vpkv.Value);
                }
                _mustBeCleared = false;
            }

            DoDraw(console);
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

        public void AddBehavior(ITvBehavior<T> behavior, Action<IBehaviorMetadata<T>> metadataAction = null)
        {
            var metadata = new BehaviorMetadata<T>(behavior);
            metadataAction?.Invoke(metadata);
            _behaviorsMetadata.Add(metadata);
        }

        public void AddBehavior(Func<BehaviorContext<T>, bool> behaviorFunc, Action<IBehaviorMetadata<T>> metadataAction = null) => AddBehavior(new ActionBehavior<T>(behaviorFunc), metadataAction);

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


        protected internal override void Update(TvConsoleEvents evts)
        {
            var updated = NeedToRedraw != RedrawNeededAction.None;
            foreach (var mdata in _behaviorsMetadata)
            {
                var ctx = new BehaviorContext<T>(State, evts, Viewport);
                if (mdata.Schedule == BehaviorSchedule.OncePerFrame
                    || (mdata.Schedule == BehaviorSchedule.OnEvents && evts != null))
                {
                    updated = mdata.Behavior.Update(ctx) || updated;
                    if (ctx.ViewportUpdated)
                    {
                        UpdateViewport(ctx.Viewport);
                    }
                }
            }

            NeedToRedraw = updated ? RedrawNeededAction.Standard : RedrawNeededAction.None;
        }


        protected override void DoDraw(VirtualConsole console)
        {
            foreach (var vpnk in _viewports)
            {
                var context = new RenderContext<T>(vpnk.Value, console, State);
                foreach (var drawer in _drawers)
                {
                    drawer?.Draw(context);
                }
            }
        }

    }
}

