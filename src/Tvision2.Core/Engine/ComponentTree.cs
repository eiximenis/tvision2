using System;
using System.Collections.Generic;
using System.Linq;
using Tvision2.Core.Components;
using Tvision2.Core.Render;
using Tvision2.Events;

namespace Tvision2.Core.Engine
{
    public class ComponentTree : IComponentTree
    {
        private readonly Dictionary<string, IComponentMetadata> _components;
        private readonly Dictionary<string, IComponentMetadata> _pendingAdds;
        private readonly Dictionary<string, IComponentMetadata> _pendingRemovals;
        private readonly List<IViewport> _viewportsToClear;
        public TuiEngine Engine { get; }
        public event EventHandler<TreeUpdatedEventArgs> ComponentAdded;
        public event EventHandler<TreeUpdatedEventArgs> ComponentRemoved;

        public IEnumerable<TvComponent> Components => _components.Values.Select(cm => cm.Component);

        public TvComponent GetComponent(string name) => _components.TryGetValue(name, out var metadata) ? metadata.Component : null;

        public bool Remove(IComponentMetadata metadata)
        {
            var name = metadata.Component.Name;
            var found = _components.ContainsKey(name);
            if (found)
            {
                _pendingRemovals.Add(name, metadata);
            }
            return found;
        }

        public bool Remove(TvComponent component) => Remove(component.Metadata);

        private void DoPendingRemovals()
        {
            foreach (var kvp in _pendingRemovals)
            {
                _components.Remove(kvp.Key);
                OnComponentRemoved(kvp.Value);
            }
            _pendingRemovals.Clear();
        }

        public ComponentTree(TuiEngine owner)
        {
            _components = new Dictionary<string, IComponentMetadata>();
            _pendingAdds = new Dictionary<string, IComponentMetadata>();
            _pendingRemovals = new Dictionary<string, IComponentMetadata>();
            _viewportsToClear = new List<IViewport>();
            Engine = owner;
        }

        public IComponentMetadata Add(IComponentMetadata metadata)
        {
            _pendingAdds.Add(metadata.Component.Name, metadata);
            return metadata;
        }

        public IComponentMetadata Add(TvComponent component) => Add(component.Metadata);

        private void DoPendingAdds()
        {
            foreach (var kvp in _pendingAdds)
            {
                _components.Add(kvp.Key, kvp.Value);
                CreateNeededBehaviors(kvp.Value.Component);
                kvp.Value.Component.Invalidate();
                OnComponentAdded(kvp.Value);
            }
            _pendingAdds.Clear();
        }

        private void CreateNeededBehaviors(TvComponent component)
        {
            var behaviorsToBeCreated = component.BehaviorsMetadatas.Where(bm => !bm.Created);
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
            ClearViewport(metadata.Component.Viewport);
            ComponentRemoved?.Invoke(this, new TreeUpdatedEventArgs(metadata));
        }

        public void ClearViewport(IViewport viewportToClear)
        {
            if (viewportToClear != null)
            {
                _viewportsToClear.Add(viewportToClear);
            }
        }

        internal void Update(TvConsoleEvents evts)
        {
            DoPendingRemovals();
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
        }

    }
}
