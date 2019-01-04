using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tvision2.Controls.Dropdown
{
    public class DropdownState : IBuildableDropDownState, IDirtyObject
    {

        private readonly List<DropDownValue> _values;
        public IEnumerable<DropDownValue> Values => _values;

        public DropdownState()
        {
            _values = new List<DropDownValue>();
        }

        public void AddValue(DropDownValue value)
        {
            _values.Add(value);
            IsDirty = true;
        }

        public bool IsDirty { get; private set; }

        public void Validate() => IsDirty = false;
    }
}
