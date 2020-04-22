using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using Tvision2.Core.Components;
using Tvision2.Core.Engine;

namespace Tvision2.Core.Components
{

    public class ListComponentCollection : IComponentsCollection
    {

        private List<TvComponent> _myComponents;
        public IEnumerable<TvComponent> Components => _myComponents;

        public int Count => _myComponents.Count;


        public ListComponentCollection()
        {
            _myComponents = new List<TvComponent>();
        }

        public void Add(TvComponent componentToAdd)
        {
            _myComponents.Add(componentToAdd);
        }


        public TvComponent GetComponent(string name)
        {
            return _myComponents.FirstOrDefault(c => c.Name == name);
        }


        public bool Remove(TvComponentMetadata metadata) => Remove(metadata.Component);

        public bool Remove(TvComponent component)
        {
            if (_myComponents.Contains(component))
            {
                _myComponents.Remove(component);
                return true;
            }
            return false;
        }

        public void Clear()
        {
            _myComponents.Clear();
        }

        public IEnumerator<TvComponent> GetEnumerator() => _myComponents.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
