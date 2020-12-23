using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tvision2.Core.Components;
using Tvision2.Core.Engine;

namespace Tvision2.Layouts.Grid
{
    internal class TvGridComponentTreeEntry
    {
        public TvComponent Component { get; }
        public ChildAlignment Alignment { get; set; }

        public bool ViewportOriginal { get; set; }

        public TvGridComponentTreeEntry(TvComponent component, ChildAlignment childAlignment)
        {
            Component = component;
            Alignment = childAlignment;
            ViewportOriginal = true;
        }
    }

    internal class TvGridComponentTree
    {
        private readonly Dictionary<(int Row, int Column), TvGridComponentTreeEntry> _childs;
        public int Count { get => _childs.Count; }

        public IEnumerable<KeyValuePair<(int Row, int Column), TvGridComponentTreeEntry>> Values => _childs;

        public TvGridComponentTree()
        {
            _childs = new Dictionary<(int Row, int Column), TvGridComponentTreeEntry>();
        }



        public void Set(int col, int row, TvComponent component, ChildAlignment alignment)
        {
            _childs.Add((row, col), new TvGridComponentTreeEntry(component, alignment));
        }

        public TvComponent GetComponent(string name) => _childs.Values.FirstOrDefault(c => c.Component.Name == name)?.Component;

        public bool Remove(TvComponent component)
        {
            (int, int)? keyToDelete = null;

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


    }
}