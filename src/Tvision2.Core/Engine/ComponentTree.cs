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

        public TvComponentMetadata Add(TvComponent component, int zindex = 0)
        {
            var metadata = MetadataFromComponent(component, zindex);
            _components.Add(component.Name, metadata);
            OnComponentAdded(metadata);
            return metadata;
        }

        private void OnComponentAdded(TvComponentMetadata metadata)
        {
            ComponentAdded?.Invoke(this, new TreeUpdatedEventArgs(metadata));
        }

        private TvComponentMetadata MetadataFromComponent(TvComponent component, int zindex)
        {
            if (zindex != 0)
            {
                component.BoxModel.ZIndex = zindex;
            }
            var newMetadata = new TvComponentMetadata(component);
            return newMetadata;
        }



        internal void Update(TvConsoleEvents evts)
        {
            foreach (var cdata in _components.Values)
            {
                cdata.Component.Update(cdata.IsResponder ? evts : null);
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


        public TvComponentMetadata FirstResponderMetadata => _responders.FirstOrDefault();

        IEnumerable<TvComponent> IComponentTree.Responders => _responders.Select(m => m.Component);


        void IComponentTree.ClearResponders()
        {
            foreach (var responder in _responders)
            {
                responder.IsResponder = false;
            }
            _responders.Clear();
        }
        void IComponentTree.AddToResponderChain(TvComponent componentToAdd)
        {
            var metadata = _components[componentToAdd.Name];
            ((IComponentTree)this).AddToResponderChain(metadata);
        }

        void IComponentTree.AddToResponderChain(TvComponentMetadata componentToAdd)
        {
            if (!componentToAdd.IsResponder)
            {
                componentToAdd.IsResponder = true;
                _responders.Add(componentToAdd);
            }
        }

        void IComponentTree.RemoveFromRespondersChain(TvComponent componentToRemove)
        {
            var metadata = _components[componentToRemove.Name];
            ((IComponentTree)this).RemoveFromRespondersChain(metadata);
        }

        void IComponentTree.RemoveFromRespondersChain(TvComponentMetadata componentToRemove)
        {
            if (componentToRemove.IsResponder)
            {
                componentToRemove.IsResponder = false;
                _responders.Remove(componentToRemove);
            }
        }

    }
}
