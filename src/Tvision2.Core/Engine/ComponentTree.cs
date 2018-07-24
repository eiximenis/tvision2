using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tvision2.Core.Components;
using Tvision2.Core.Render;
using Tvision2.Events;

namespace Tvision2.Core.Engine
{
    public class ComponentTree : IComponentTree
    {
        private readonly Dictionary<string, IComponentMetadata> _components;
        private readonly List<IViewport> _viewportsToClear;
        public TuiEngine Engine { get; }
        public event EventHandler<TreeUpdatedEventArgs> ComponentAdded;
        public event EventHandler<TreeUpdatedEventArgs> ComponentRemoved;

        public IEnumerable<TvComponent> Components => _components.Values.Select(cm => cm.Component);

        public TvComponent GetComponent(string name) => _components.TryGetValue(name, out IComponentMetadata metadata) ? metadata.Component : null;

        public bool Remove (IComponentMetadata metadata)
        {
            var name = metadata.Component.Name;
            var found = _components.ContainsKey(name);
            if (found)
            {
                _components.Remove(name);
                OnComponentRemoved(metadata);
            }
            return found;
        }

        public bool Remove(TvComponent component) => Remove(component.Metadata);

        public ComponentTree(TuiEngine owner)
        {
            _components = new Dictionary<string, IComponentMetadata>();
            _viewportsToClear = new List<IViewport>();
            Engine = owner;
        }

        public IComponentMetadata Add (IComponentMetadata metadata)
        {
            _components.Add(metadata.Component.Name, metadata);
            CreateNeededBehaviors(metadata.Component);
            OnComponentAdded(metadata);
            return metadata;
        }

        private void CreateNeededBehaviors(TvComponent component)
        {
            var behaviorsToBeCreated = component.BehaviorsMetadatas.Where(bm => !bm.Created);
            foreach (var bm in behaviorsToBeCreated)
            {
                bm.CreateBehavior(Engine.ServiceProvider);
            }
        }

        public IComponentMetadata Add(TvComponent component)
        {
            return Add(component.Metadata);
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
