using System;
using System.Collections.Generic;
using System.Linq;
using Tvision2.Core.Components;

namespace Tvision2.Core.Engine
{
    public class KeyedComponentsTree<T> : IComponentTree
        where T : struct
    {
        private T _currentKey;
        private readonly IComponentTree _parent;
        private Dictionary<T, ListComponentTree> _myComponents;

        public IEnumerable<TvComponent> ComponentsForKey(T key) 
            => _myComponents.TryGetValue(key, out ListComponentTree childTree) ? childTree.Components : Enumerable.Empty<TvComponent>();

        public int ItemsCount => _myComponents.Count;

        TuiEngine IComponentTree.Engine => _parent.Engine;

        public IEnumerable<KeyValuePair<T, ListComponentTree>> Items => _myComponents;

        public event EventHandler<TreeUpdatedEventArgs> ComponentAdded;
        public event EventHandler<TreeUpdatedEventArgs> ComponentRemoved;

        public T CurrentKey => _currentKey;

        public IComponentTree SetKey(T newkey)
        {
            _currentKey = newkey;
            return this;
        }

        public KeyedComponentsTree(IComponentTree root)
        {
            _myComponents = new Dictionary<T, ListComponentTree>();
            _parent = root;
        }

        IComponentMetadata IComponentTree.Add(TvComponent component)
        {
            if (_myComponents.ContainsKey(_currentKey))
            {
                _myComponents[_currentKey].Add(component);
            }
            else
            {
                var child = new ListComponentTree(_parent);
                _myComponents.Add(_currentKey, child);
                child.Add(component);
            }

            OnComponentAdded(component.Metadata);
            return component.Metadata;
        }

        IEnumerable<TvComponent> IComponentTree.Components => _myComponents.Values.SelectMany(c => c.Components);

        TvComponent IComponentTree.GetComponent(string name)
        {
            return ((IComponentTree)this).Components.FirstOrDefault(c => c.Name == name);
        }

        private void OnComponentAdded(IComponentMetadata metadata)
        {
            ComponentAdded?.Invoke(this, new TreeUpdatedEventArgs(metadata));
        }

        private void OnComponentRemoved(IComponentMetadata metadata)
        {
            ComponentRemoved?.Invoke(this, new TreeUpdatedEventArgs(metadata));
        }

        bool IComponentTree.Remove(TvComponent component)
        {
            foreach (var key in _myComponents.Keys)
            {
                var childTree = _myComponents[key];
                if (childTree.Remove(component))
                {
                    OnComponentRemoved(component.Metadata);
                    if (!childTree.Components.Any())
                    {
                        _myComponents.Remove(key);
                    }

                    return true;
                }
            }

            return false;
        }

        public void Clear()
        {
            foreach (var child in _myComponents.Values)
            {
                child.Clear();
            }
        }
    }
}
