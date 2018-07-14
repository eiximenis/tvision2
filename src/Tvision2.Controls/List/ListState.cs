using System.Collections.Generic;

namespace Tvision2.Controls.List
{
    public class ListState : IDirtyObject
    {
        public bool IsDirty { get; private set; }

        public void Validate() => IsDirty = false;

        public IEnumerable<string> Values { get; }

        public ListState(IEnumerable<string> values)
        {
            Values = values;
        }
    }

}