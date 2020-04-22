using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tvision2.Core.Components;

namespace Tvision2.Layouts
{
    public class KeyedComponentsCollection<T> : IComponentsCollection
        where T : struct
    {
        private Dictionary<T, ListComponentCollection> _myComponents;

        public IEnumerable<TvComponent> ComponentsForKey(T key) 
            => _myComponents.TryGetValue(key, out ListComponentCollection childTree) ? childTree.Components : Enumerable.Empty<TvComponent>();

        public int Count => _myComponents.Count;

        public IEnumerable<KeyValuePair<T, ListComponentCollection>> Items => _myComponents;

        public T CurrentKey { get; private set; }

        public void SetKey(T newkey)
        {
            CurrentKey = newkey;
        }

        public KeyedComponentsCollection()
        {
            _myComponents = new Dictionary<T, ListComponentCollection>();
        }


        public void Add(TvComponent componentToAdd)
        {
            if (_myComponents.ContainsKey(CurrentKey))
            {
                _myComponents[CurrentKey].Add(componentToAdd);
            }
            else
            {
                var child = new ListComponentCollection();
                _myComponents.Add(CurrentKey, child);
                child.Add(componentToAdd);
            }
        }

        public bool Remove(TvComponent component)
        {
            foreach (var key in _myComponents.Keys)
            {
                var childTree = _myComponents[key];
                if (childTree.Remove(component))
                {
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

        public IEnumerator<TvComponent> GetEnumerator() => _myComponents.Values.SelectMany(v => v).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();


    }
}
