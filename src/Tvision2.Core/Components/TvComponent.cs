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


    public abstract class TvComponent : IAdaptativeDrawingTarget
    {

        readonly struct AdaptativeDrawerDefinition
        {
            public Func<IViewport, bool> Selector { get; }
            public List<ITvDrawer> Drawers { get; }

            public AdaptativeDrawerDefinition(Func<IViewport, bool> selector)
            {
                Selector = selector;
                Drawers = new List<ITvDrawer>();
            }
        }

        private readonly Dictionary<Guid, List<ITvDrawer>> _adaptativeDrawers;
        private readonly List<AdaptativeDrawerDefinition> _adaptativeDrawersDefinitions;
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

        protected readonly List<ITvDrawer> _defaultDrawers;

        

        protected readonly List<IBehaviorMetadata> _behaviorsMetadata;

        public TvComponent(string name, Action<IConfigurableComponentMetadata> configAction = null)
        {
            ComponentId = Guid.NewGuid();
            _metadata = new TvComponentMetadata(this);
            configAction?.Invoke(_metadata);
            _defaultDrawers = new List<ITvDrawer>();
            _behaviorsMetadata = new List<IBehaviorMetadata>();
            _viewports = new Dictionary<Guid, IViewport>();
            Name = string.IsNullOrEmpty(name) ? $"TvComponent-{ComponentId}" : name.Replace("<$>", ComponentId.ToString());
            _adaptativeDrawers = new Dictionary<Guid, List<ITvDrawer>>();
            _adaptativeDrawersDefinitions = new List<AdaptativeDrawerDefinition>();
        }


        public void RemoveAllBehaviors()
        {
            _behaviorsMetadata.Clear();
        }

        public void RemoveAllDrawers()
        {
            _defaultDrawers.Clear();
            _adaptativeDrawers.Clear();
            _adaptativeDrawersDefinitions.Clear();
        }

        public Guid AddViewport(IViewport viewport)
        {
            var key = _viewports.Any() ? Guid.NewGuid() : Guid.Empty;
            _viewports.Add(key, viewport);
            UpdateAdaptativeDrawersForUpdatedViewport(key, viewport);
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
                UpdateAdaptativeDrawersForUpdatedViewport(guid, newViewport);
                _metadata?.OnViewportChanged(guid, oldvp, newViewport);
            }
            else if (addIfNotExists)
            {
                _viewports.Add(guid, newViewport);
                UpdateAdaptativeDrawersForUpdatedViewport(guid, newViewport);
                _metadata?.OnViewportChanged(guid, null, newViewport);
            }
        }



        public IAdaptativeDrawingTarget IfViewport(Func<IViewport, bool> condition)
        {
            AddDrawerDefinition(condition);
            return this;
        }

        IAdaptativeDrawingTarget IAdaptativeDrawingTarget.AddDrawer(ITvDrawer drawer)
        {
            AddDrawerToCurrentAdaptativeDrawerDefinition(drawer);
            UpdateAdaptativeDrawersForNewDrawerInCurrentDefinition(drawer);
            return this;
        }


        public void AddDrawer(ITvDrawer drawer) => _defaultDrawers.Add(drawer);

        public void InsertDrawerAt(ITvDrawer drawer, int index)
        {
            _defaultDrawers.Insert(0, drawer);
        }

        protected internal abstract void Update(UpdateContext ctx);

        protected abstract void DoDraw(VirtualConsole console, ComponentTreeNode node);

        protected internal void Draw(VirtualConsole console, ComponentTreeNode node)
        {
            DoDraw(console, node);
            NeedToRedraw = RedrawNeededAction.None;
        }

        protected void AddDrawerDefinition(Func<IViewport, bool> selector)
        {
            _adaptativeDrawersDefinitions.Add(new AdaptativeDrawerDefinition(selector));
        }

        protected void AddDrawerToCurrentAdaptativeDrawerDefinition(ITvDrawer drawer)
        {
            _adaptativeDrawersDefinitions[_adaptativeDrawersDefinitions.Count - 1].Drawers.Add(drawer);
        }

        protected IEnumerable<ITvDrawer> GetAdaptativeDrawersForViewport(Guid id)
        {
            return _adaptativeDrawers.TryGetValue(id, out var drawers) ? drawers : Enumerable.Empty<ITvDrawer>();
        }


        private void UpdateAdaptativeDrawersForUpdatedViewport(Guid guid, IViewport newViewport)
        {
            var drawers = new List<ITvDrawer>();
            foreach (var definition in _adaptativeDrawersDefinitions)
            {
                if (definition.Selector(newViewport))
                {
                    drawers.AddRange(definition.Drawers);
                }
            }

            if (_adaptativeDrawers.ContainsKey(guid))
            {
                _adaptativeDrawers[guid] = drawers;
            }
            else
            {
                _adaptativeDrawers.Add(guid, drawers);
            }
        }

        protected void UpdateAdaptativeDrawersForNewDrawerInCurrentDefinition(ITvDrawer drawer)
        {
            var currentDefinition = _adaptativeDrawersDefinitions[_adaptativeDrawersDefinitions.Count - 1];
            foreach (var (key, viewport) in _viewports)
            {
                if (currentDefinition.Selector(viewport))
                {
                    if (!_adaptativeDrawers.ContainsKey(key))
                    {
                        _adaptativeDrawers.Add(key, new List<ITvDrawer>());
                    }
                    _adaptativeDrawers[key].Add(drawer);
                }
            }
        }

    }

    public sealed class TvComponent<T> : TvComponent, IAdaptativeDrawingTarget<T>
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


        public void AddBehavior<TB>(TB behavior, Action<BehaviorMetadata<T, TB>> metadataAction = null)
            where TB: ITvBehavior<T>
        {
            var metadata = new BehaviorMetadata<T, TB>(behavior);
            metadataAction?.Invoke(metadata);
            _behaviorsMetadata.Add(metadata);
        }


        public void AddBehavior<TB>(Action<FactoryBehaviorMetadata<TB, T>> metadataAction = null)
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


        public new IAdaptativeDrawingTarget<T> IfViewport(Func<IViewport, bool> condition)
        {
            AddDrawerDefinition(condition);
            return this;
        }

        IAdaptativeDrawingTarget<T> IAdaptativeDrawingTarget<T>.AddDrawer(ITvDrawer<T> drawer)
        {
            AddDrawerToCurrentAdaptativeDrawerDefinition(drawer);
            UpdateAdaptativeDrawersForNewDrawerInCurrentDefinition(drawer);
            return this;
        }

        IAdaptativeDrawingTarget<T> IAdaptativeDrawingTarget<T>.AddDrawer(Action<RenderContext<T>> action)
        {
            return ((IAdaptativeDrawingTarget<T>)this).AddDrawer(new ActionDrawer<T>(action));
        }


        public TvComponent<T> AddDrawer(Action<RenderContext<T>> action) => AddDrawer(new ActionDrawer<T>(action));

        public TvComponent<T> AddDrawer(ITvDrawer<T> drawer)
        {
            _defaultDrawers.Add(drawer);
            return this;
        }


        protected internal override void Update(UpdateContext ctx)
        {
            var updated = NeedToRedraw != RedrawNeededAction.None;
            foreach (var mdata in _behaviorsMetadata)
            {
                var evts = ctx.Events;
                var behaviorCtx = new BehaviorContext<T>(State, evts, ctx.ComponentLocator);
                if (mdata.Schedule == BehaviorSchedule.OncePerFrame
                    || (mdata.Schedule == BehaviorSchedule.OnEvents && evts.HasEvents))
                {
                    updated =  ((IBehaviorMetadata<T>)mdata).Behavior.Update(behaviorCtx) || updated;
                }
            }
            NeedToRedraw = updated ? RedrawNeededAction.Standard : RedrawNeededAction.None;
        }


        protected override void DoDraw(VirtualConsole console, ComponentTreeNode node)
        {
            foreach (var vpnk in _viewports)
            {
                var context = new RenderContext<T>(vpnk.Value, console,  node, NeedToRedraw, State);
                var adaptativeDrawers = GetAdaptativeDrawersForViewport(vpnk.Key);
                var viewportDrawed = false;
                foreach (var drawer in adaptativeDrawers)
                {
                    drawer?.Draw(context);
                    viewportDrawed = true;
                }
                if (!viewportDrawed)
                {
                    foreach (var drawer in _defaultDrawers)
                    {
                        drawer?.Draw(context);
                    }
                }
            }
        }

    }
}

