using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tvision2.Core.Components;

namespace Tvision2.Core.Engine
{
    public class ListComponentTree : IComponentTree
    {
        private readonly IComponentTree _parent;
        private List<TvComponent> _myComponents;
        public IEnumerable<TvComponent> Components => _myComponents;

        public int Count => _myComponents.Count;

        public TuiEngine Engine => _parent.Engine;

        public event EventHandler<TreeUpdatedEventArgs> ComponentAdded;
        public event EventHandler<TreeUpdatedEventArgs> ComponentRemoved;

        public ListComponentTree(IComponentTree root)
        {
            _myComponents = new List<TvComponent>();
            _parent = root;
        }

        public IComponentMetadata Add(TvComponent component)
        {
            var metadata = _parent.Add(component);
            _myComponents.Add(component);
            OnComponentAdded(component.Metadata);
            return metadata;
        }

        public IComponentMetadata Add(IComponentMetadata metadata)
        {
            _parent.Add(metadata);
            _myComponents.Add(metadata.Component);
            OnComponentAdded(metadata);
            return metadata;
        }

        public TvComponent GetComponent(string name)
        {
            return _myComponents.FirstOrDefault(c => c.Name == name);
        }

        private void OnComponentAdded(IComponentMetadata metadata)
        {
            ComponentAdded?.Invoke(this, new TreeUpdatedEventArgs(metadata));
        }

        private void OnComponentRemoved(IComponentMetadata metadata)
        {
            ComponentRemoved?.Invoke(this, new TreeUpdatedEventArgs(metadata));
        }

        public bool Remove(IComponentMetadata metadata) => Remove(metadata.Component);

        public bool Remove(TvComponent component)
        {
            if (_myComponents.Contains(component))
            {
                _parent.Remove(component);
                _myComponents.Remove(component);
                OnComponentRemoved(component.Metadata);
                return true;
            }

            return false;
        }

    }
}
