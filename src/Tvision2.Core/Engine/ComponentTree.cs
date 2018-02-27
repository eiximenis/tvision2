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


        public ComponentTree(TuiEngine owner)
        {
            _components = new Dictionary<string, TvComponentMetadata>();
            _responders = new List<TvComponentMetadata>();
            Engine = owner;
        }

        public TvComponentMetadata Add(TvComponent component, int zindex = 0)
        {
            var metadata = MetadataFromComponent(component, zindex);
            _components.Add(component.Name, metadata);
            return metadata;
        }



        private TvComponentMetadata MetadataFromComponent(TvComponent component, int zindex)
        {
            if (zindex != 0)
            {
                component.Style.ZIndex = zindex;
            }
            var viewport = new Viewport(component.Style);
            var newMetadata = new TvComponentMetadata(component, viewport);
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
                .Where(c => force || c.Component.IsDirty))
            {
                cdata.Component.Draw(cdata.Viewport, console);
            }
        }


        IEnumerable<TvComponent> IComponentTree.Responders => _responders.Select(m => m.Component);


        void IComponentTree.ClearResponders() => _responders.Clear();
        void IComponentTree.AddToResponderChain(TvComponent componentToAdd)
        {
            var metadata = _components[componentToAdd.Name];
            _responders.Add(metadata);
        }

        void IComponentTree.RemoveFromRespondersChain(TvComponent componentToRemove)
        {
            var metadata = _components[componentToRemove.Name];
            _responders.Remove(metadata);
        }


    }
}
