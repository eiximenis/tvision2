using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Tvision2.Core.Components;
using Tvision2.Core.Render;
using Tvision2.Events;

namespace Tvision2.Core.Engine
{
    public class ComponentTree : IComponentTree
    {
        private readonly Dictionary<string, TvComponentMetadata> _components;
        private readonly Dictionary<string, TvComponentMetadata> _pendingAdds;
        private readonly Dictionary<string, TvComponentMetadata> _pendingRemovalsPhase1;
        private readonly Dictionary<string, TvComponentMetadata> _pendingRemovalsPhase2;
        private readonly List<IViewport> _viewportsToClear;
        public TuiEngine Engine { get; }
        public event EventHandler<TreeUpdatedEventArgs> ComponentAdded;
        public event EventHandler<TreeUpdatedEventArgs> ComponentRemoved;

        public IEnumerable<TvComponent> Components => _components.Values.Select(cm => cm.Component);

        public TvComponent GetComponent(string name) => _components.TryGetValue(name, out var metadata) ? metadata.Component : null;


        public bool Remove(TvComponent component)
        {
            var name = component.Name;
            var found = _components.ContainsKey(name);
            if (found)
            {
                _pendingRemovalsPhase1.Add(name, component.Metadata as TvComponentMetadata);
            }
            return found;
        }

        private void DoPendingRemovalsPhase1()
        {
            if (_pendingRemovalsPhase1.Count == 0)
            {
                return;
            }
            var toDelete = _pendingRemovalsPhase1.ToArray();
            _pendingRemovalsPhase1.Clear();
            foreach (var kvp in toDelete)
            {   
                var canBeUnmounted = kvp.Value.CanBeUnmountedFrom(this);
                if (canBeUnmounted)
                {
                    _pendingRemovalsPhase2.Add(kvp.Key, kvp.Value);
                    _viewportsToClear.AddRange(kvp.Value.Component.Viewports.Select(x => x.Value));
                    _components.Remove(kvp.Key);
                }
            }
        }

        private void DoPendingRemovalsPhase2()
        {
            if (_pendingRemovalsPhase2.Count == 0)
            {
                return;
            }

            foreach (var kvp in _pendingRemovalsPhase2)
            {
                kvp.Value.UnmountedFrom(this);
                OnComponentRemoved(kvp.Value);
            }

            _pendingRemovalsPhase2.Clear();
        }

        public ComponentTree(TuiEngine owner)
        {
            _components = new Dictionary<string, TvComponentMetadata>();
            _pendingAdds = new Dictionary<string, TvComponentMetadata>();
            _pendingRemovalsPhase1 = new Dictionary<string, TvComponentMetadata>();
            _pendingRemovalsPhase2 = new Dictionary<string, TvComponentMetadata>();
            _viewportsToClear = new List<IViewport>();
            Engine = owner;
        }

        public IComponentMetadata Add(TvComponent component)
        {
            _pendingAdds.Add(component.Name, component.Metadata as TvComponentMetadata);
            return component.Metadata;
        }

        private void DoPendingAdds()
        {
            if (_pendingAdds.Count == 0)
            {
                return;
            }

            var toAdd = _pendingAdds.ToArray();
            _pendingAdds.Clear();

            foreach (var kvp in toAdd)
            {
                _components.Add(kvp.Key, kvp.Value);
                CreateNeededBehaviors(kvp.Value.Component);
                kvp.Value.MountedTo(this);
                kvp.Value.Component.Invalidate();
                OnComponentAdded(kvp.Value);
            }
        }

        private void CreateNeededBehaviors(TvComponent component)
        {
            var behaviorsToBeCreated = component.BehaviorsMetadatas.Where(bm => !bm.Created).ToList();
            foreach (var bm in behaviorsToBeCreated)
            {
                bm.CreateBehavior(Engine.ServiceProvider);
            }
        }

        private void OnComponentAdded(IComponentMetadata metadata)
        {
            ComponentAdded?.Invoke(this, new TreeUpdatedEventArgs(metadata));
        }

        private void OnComponentRemoved(IComponentMetadata metadata)
        {
            ComponentRemoved?.Invoke(this, new TreeUpdatedEventArgs(metadata));
        }

        public void ClearViewport(IViewport viewportToClear)
        {
            if (viewportToClear != null)
            {
                _viewportsToClear.Add(viewportToClear);
            }
        }

        public void Clear()
        {
            foreach (var child in Components)
            {
                Remove(child);
            }
        }

        internal void Update(TvConsoleEvents evts)
        {
            DoPendingRemovalsPhase1();
            DoPendingAdds();
            foreach (var cdata in _components.Values)
            {
                cdata.Component.Update(evts);
            }
        }

        internal void Draw(VirtualConsole console, bool force)
        {
            foreach (var viewport in _viewportsToClear)
            {
                console.Clear(viewport);
            }

            foreach (var cdata in _components.Values
                .Where(c => force || c.Component.NeedToRedraw != RedrawNeededAction.None))
            {
                cdata.Component.Draw(console);
            }

            _viewportsToClear.Clear();

            DoPendingRemovalsPhase2();
        }

    }
}
