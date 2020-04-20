﻿using System;
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

        public event EventHandler<TreeUpdatedEventArgs> ComponentAdded;
        public event EventHandler<TreeUpdatedEventArgs> ComponentRemoved;

        public ListComponentTree(IComponentTree root)
        {
            _myComponents = new List<TvComponent>();
            _parent = root;
        }


        public IComponentMetadata AddAfter(TvComponent componentToAdd, TvComponent componentBefore, Action<ITuiEngine> afterAddAction = null)
        {
            var metadata = _parent.AddAfter(componentToAdd, componentBefore,afterAddAction);
            _myComponents.Add(componentToAdd);
            OnComponentAdded(componentToAdd.Metadata);
            return metadata;
        }

        public IComponentMetadata Add(TvComponent component, Action<ITuiEngine> afterAddAction)
        {
            var metadata = _parent.Add(component, afterAddAction);
            _myComponents.Add(component);
            OnComponentAdded(component.Metadata);
            return metadata;
        }

        public IComponentMetadata AddAsChild(TvComponent componentToAdd, TvComponent parent, Action<ITuiEngine> afterAddAction = null)
        {
            var metadata = _parent.AddAsChild(componentToAdd, parent, afterAddAction);
            _myComponents.Add(componentToAdd);
            OnComponentAdded(componentToAdd.Metadata);
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

        public void Clear()
        {
            var cachedChilds = _myComponents.ToList();
            foreach (var component in cachedChilds)
            {
                Remove(component);
            }
        }
    }
}
