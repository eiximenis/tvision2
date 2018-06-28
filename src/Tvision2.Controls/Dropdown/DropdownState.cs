using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Controls.Dropdown
{
    public class DropdownState : IDirtyObject
    {

        public IEnumerable<string> Values => new[] { "v1", "v2", "v3" };

        public bool IsDirty { get; private set; }

        public void Validate() => IsDirty = false;
    }
}
