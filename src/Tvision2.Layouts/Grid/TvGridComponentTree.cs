using System;
using System.Collections.Generic;
using System.Linq;
using Tvision2.Core.Components;
using Tvision2.Core.Engine;

namespace Tvision2.Layouts.Grid
{
    internal class TvGridComponentTree : IComponentTree
    {
        private readonly Dictionary<(int Row, int Column), TvComponent> _childs;
        private readonly IComponentTree _root;


        public int CurrentRow { get; set; }
        public int CurrentColumn { get; set; }

        public IEnumerable<KeyValuePair<(int Row, int Column), TvComponent>> Values => _childs;

        public TvGridComponentTree(IComponentTree root)
        {
            _childs = new Dictionary<(int Row, int Column), TvComponent>();
            _root = root;
            CurrentColumn = 0;
            CurrentRow = 0;
        }

        public TuiEngine Engine => _root.Engine;

        public IEnumerable<TvComponent> Components => _childs.Values;

        public event EventHandler<TreeUpdatedEventArgs> ComponentAdded;
        public event EventHandler<TreeUpdatedEventArgs> ComponentRemoved;

        public IComponentMetadata Add(IComponentMetadata metadata)
        {
            _childs.Add((CurrentRow, CurrentColumn), metadata.Component);
            _root.Add(metadata);
            OnComponentAdded(metadata);
            return metadata;
        }

        public IComponentMetadata Add(TvComponent component)
        {
            _childs.Add((CurrentRow, CurrentColumn), component);
            _root.Add(component);
            OnComponentAdded(component.Metadata);
            return component.Metadata;
        }

        private void OnComponentAdded(IComponentMetadata metadata)
        {
            var handler = ComponentAdded;
            handler?.Invoke(this, new TreeUpdatedEventArgs(metadata));
        }

        private void OnComponentRemoved(IComponentMetadata metadata)
        {
            var handler = ComponentRemoved;
            handler?.Invoke(this, new TreeUpdatedEventArgs(metadata));
        }

        public TvComponent GetComponent(string name) => _childs.Values.FirstOrDefault(c => c.Name == name);

        public bool Remove(IComponentMetadata metadata)
        {
            var cmp = metadata.Component;
            (int, int)? keyToDelete = null;
            
            foreach (var child in _childs)
            {
                if (child.Value == cmp)
                {
                    keyToDelete = child.Key;
                    break;
                }
            }
            if (keyToDelete.HasValue)
            {
                _childs.Remove(keyToDelete.Value);
                _root.Remove(metadata);
                OnComponentRemoved(metadata);
                return true;
            }

            return false;
        }

        public bool Remove(TvComponent component) => Remove(component.Metadata);

        public void Clear()
        {
            var toDelete = _childs.Values.ToList();
            foreach (var child in toDelete)
            {
                Remove(child);
            }
        }
    }
}