using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Controls.Styles;
using Tvision2.Core.Render;

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
