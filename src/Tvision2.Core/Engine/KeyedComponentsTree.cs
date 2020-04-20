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


        public IComponentMetadata AddAsChild(TvComponent componentToAdd, TvComponent parent, Action<ITuiEngine> afterAddAction = null)
        {
            throw new NotSupportedException();
        }

        public IComponentMetadata AddAfter(TvComponent componentToAdd, TvComponent componentBefore, Action<ITuiEngine> afterAddAction = null)
        {
            if (_myComponents.ContainsKey(_currentKey))
            {
                _myComponents[_currentKey].Add(componentToAdd, afterAddAction);
            }
            else
            {
                var child = new ListComponentTree(_parent);
                _myComponents.Add(_currentKey, child);
                if (componentBefore == null)
                {
                    child.Add(componentToAdd, afterAddAction);
                }
                else
                {
                    child.AddAfter(componentToAdd, componentBefore, afterAddAction);

                }
   
            }

            OnComponentAdded(componentToAdd.Metadata);
            return componentToAdd.Metadata;
        }

        IComponentMetadata IComponentTree.Add(TvComponent component, Action<ITuiEngine> afterAddAction) => AddAfter(component, null, afterAddAction);

        IEnumerable<TvComponent> IReadonlyComponentTree.Components => _myComponents.Values.SelectMany(c => c.Components);

        TvComponent IReadonlyComponentTree.GetComponent(string name)
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
