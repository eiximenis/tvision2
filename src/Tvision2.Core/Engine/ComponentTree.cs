using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tvision2.Core.Components;
using Tvision2.Core.Render;
using Tvision2.Events;

namespace Tvision2.Core.Engine
{
    class ComponentTree : IComponentTree
    {
        private readonly Dictionary<string, IComponentMetadata> _components;
        private List<IComponentMetadata> _responders;
        public TuiEngine Engine { get; }
        public event EventHandler<TreeUpdatedEventArgs> ComponentAdded;

        public IEnumerable<TvComponent> Components => _components.Values.Select(cm => cm.Component);

        public TvComponent GetComponent(string name) => _components.TryGetValue(name, out IComponentMetadata metadata) ? metadata.Component : null;

        public ComponentTree(TuiEngine owner)
        {
            _components = new Dictionary<string, IComponentMetadata>();
            _responders = new List<IComponentMetadata>();
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

        internal void Update(TvConsoleEvents evts)
        {
            foreach (var cdata in _components.Values)
            {
                cdata.Component.Update(evts);
            }
        }



        internal void Draw(VirtualConsole console, bool force)
        {
            foreach (var cdata in _components.Values
                .Where(c => force || c.Component.NeedToRedraw))
            {
                cdata.Component.Draw(console);
            }
        }

    }
}
