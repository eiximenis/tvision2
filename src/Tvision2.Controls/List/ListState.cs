using System;
using System.Collections.Generic;
using System.Linq;

namespace Tvision2.Controls.List
{
    public class ListState : IDirtyObject
    {
        private readonly List<string> _values;
        public bool IsDirty { get; private set; }

        public void Validate() => IsDirty = false;

        public IEnumerable<string> Values => _values;

        public ListState(IEnumerable<string> values)
        {
            _values = values.ToList();
        }

        public void Clear()
        {
            _values.Clear();
            IsDirty = true;
        }

        public void AddRange(IEnumerable<string> items)
        {
            _values.AddRange(items);
            IsDirty = true;
        }

        public void AddRange(object items)
        {
            throw new NotImplementedException();
        }
    }

}