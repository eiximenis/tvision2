using System;

namespace Tvision2.Controls.Checkbox
{
    public class CheckboxState : IDirtyObject
    {
        public bool IsDirty { get; private set; }
        void IDirtyObject.Validate() => IsDirty = false;
        private TvCheckboxState _checked;
        public TvCheckboxState Checked
        {
            get => _checked;
            set { _checked = value; IsDirty = true; }
        }
    }
}
