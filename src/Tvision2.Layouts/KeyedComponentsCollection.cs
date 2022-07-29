using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tvision2.Core.Components;

namespace Tvision2.Layouts
{
    public class KeyedComponentCollectionEntry
    {
        public TvComponent Component { get; }
        public ChildAlignment Alignment { get; set; }

        public bool ViewportOriginal { get; set; }

        public KeyedComponentCollectionEntry(TvComponent component, ChildAlignment childAlignment)
        {
            Component = component;
            Alignment = childAlignment;
            ViewportOriginal = true;
        }
    }

    public class KeyedComponentsCollection<TK>
        where TK : struct
    {
        private Dictionary<TK, KeyedComponentCollectionEntry> _childs;


        public int Count => _childs.Count;

        public IEnumerable<KeyValuePair<TK, KeyedComponentCollectionEntry>> Values => _childs;



        public KeyedComponentsCollection()
        {
            _childs = new Dictionary<TK, KeyedComponentCollectionEntry>();
        }


        public void Set(TK key, TvComponent componentToAdd, ChildAlignment alignment)
        {

            _childs.Add(key, new KeyedComponentCollectionEntry(componentToAdd, alignment));
        }

        public bool Remove(TvComponent component)
        {
            TK? keyToDelete = null;

            foreach (var child in _childs)
            {
                if (child.Value.Component == component)
                {
                    keyToDelete = child.Key;
                    break;
                }
            }

            if (keyToDelete.HasValue)
            {
                _childs.Remove(keyToDelete.Value);
                return true;
            }

            return false;
        }

        public void Clear()
        {
            _childs.Clear();
        }

        public IEnumerator<TvComponent> GetEnumerator() => _childs.Values.Select(v => v.Component).GetEnumerator();

        public KeyedComponentCollectionEntry? Get(TK key) => _childs.TryGetValue(key, out var value) ? value : null;


    }
}
