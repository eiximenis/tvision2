﻿using System;
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
        private readonly TvComponentMetadata _metadata;
        protected Dictionary<Guid, IViewport> _viewports;
        public bool NeedToRedraw { get; protected set; }
        public string Name { get; }
        public void Invalidate() => NeedToRedraw = true;
        public IComponentMetadata Metadata => _metadata;

        protected readonly List<ITvDrawer> _drawers;

        public TvComponent(string name)
        {
            _metadata = new TvComponentMetadata(this);
            _drawers = new List<ITvDrawer>();
            _viewports = new Dictionary<Guid, IViewport>();
            Name = string.IsNullOrEmpty(name) ? $"TvComponent-{Guid.NewGuid().ToString()}" : name;
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
        public IEnumerable<IViewport> Viewports => _viewports.Values;

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

        protected internal abstract void Update(TvConsoleEvents evts);
        protected internal abstract void Draw(VirtualConsole console);
     
    }

    public class TvComponent<T> : TvComponent
    {
        private readonly List<BehaviorMetadata<T>> _behaviorsMetadata;
        public T State { get; private set; }

        public bool HasFocus { get; internal set; }

        public void SetState(T newState)
        {
            var oldState = State;
            State = newState;
            NeedToRedraw = Object.ReferenceEquals(oldState, State);
        }


        public TvComponent(T initialState, string name = null) : base(name)
        {
            NeedToRedraw = false;
            _behaviorsMetadata = new List<BehaviorMetadata<T>>();
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

        public TvComponent<T> AddDrawer(Action<RenderContext<T>> action) => AddDrawer(new ActionDrawer<T>(action));

        public TvComponent<T> AddDrawer(ITvDrawer<T> drawer)
        {
            _drawers.Add(drawer);
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
            foreach (var vpnk in _viewports)
            {
                var context = new RenderContext<T>(vpnk.Value, console, State);
                foreach (var drawer in _drawers)
                {
                    drawer?.Draw(context);
                }
            }
            NeedToRedraw = false;

        }

    }
}

