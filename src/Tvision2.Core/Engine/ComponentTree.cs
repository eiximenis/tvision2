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
        private readonly Dictionary<string, TvComponentMetadata> _components;
        private List<TvComponentMetadata> _responders;

        public TuiEngine Engine { get; }

        public event EventHandler<TreeUpdatedEventArgs> ComponentAdded;

        public ComponentTree(TuiEngine owner)
        {
            _components = new Dictionary<string, TvComponentMetadata>();
            _responders = new List<TvComponentMetadata>();
            Engine = owner;
        }

        public TvComponentMetadata Add (TvComponentMetadata metadata)
        {
            _components.Add(metadata.Component.Name, metadata);
            OnComponentAdded(metadata);
            return metadata;
        }

        public TvComponentMetadata Add(TvComponent component)
        {
            var metadata = MetadataFromComponent(component);
            return Add(metadata);
        }

        private void OnComponentAdded(TvComponentMetadata metadata)
        {
            ComponentAdded?.Invoke(this, new TreeUpdatedEventArgs(metadata));
        }

        private TvComponentMetadata MetadataFromComponent(TvComponent component)
        {
            var newMetadata = new TvComponentMetadata(component, Engine.ConsoleDriver);
            return newMetadata;
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
