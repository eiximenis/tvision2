using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tvision2.Core.Components;
using Tvision2.Core.Engine;

namespace Tvision2.Layouts.Grid
{
    internal class TvGridComponentTree : IComponentsCollection
    {
        private readonly Dictionary<(int Row, int Column), TvComponent> _childs;
        public int Count { get => _childs.Count; }
        public int CurrentRow { get; set; }
        public int CurrentColumn { get; set; }

        public IEnumerable<KeyValuePair<(int Row, int Column), TvComponent>> Values => _childs;

        public TvGridComponentTree()
        {
            _childs = new Dictionary<(int Row, int Column), TvComponent>();
            CurrentColumn = 0;
            CurrentRow = 0;
        }

        public IEnumerable<TvComponent> Components => _childs.Values;


        public void Add(TvComponent component)
        {
            _childs.Add((CurrentRow, CurrentColumn), component);
        }

        public TvComponent GetComponent(string name) => _childs.Values.FirstOrDefault(c => c.Name == name);

        public bool Remove(TvComponent component)
        {
            (int, int)? keyToDelete = null;

            foreach (var child in _childs)
            {
                if (child.Value == component)
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

        public IEnumerator<TvComponent> GetEnumerator() => _childs.Values.GetEnumerator();

       IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();


    }
}