using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Render;
using Tvision2.Core.Styles;

namespace Tvision2.Controls.Checkbox
{
    public class CheckboxState : TvControlData
    {
        public CheckboxState(string name = null) : base(new AppliedStyle(ClippingMode.Clip), name ?? "Check" + Guid.NewGuid().ToString())
        {
            Style.Columns = 3;
            _checked = TvCheckboxState.Unchecked;
        }



        private TvCheckboxState _checked;
        public TvCheckboxState Checked
        {
            get => _checked;
            set { _checked = value; IsDirty = true; }

        }


    }
}
