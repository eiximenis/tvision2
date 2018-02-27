using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Components;
using Tvision2.Core.Render;

namespace Tvision2.Controls.Checkbox
{
    public class TvCheckbox : TvControlString<CheckboxState>
    {
        public TvCheckbox(CheckboxState state) : base(state, state)
        {
        }

        protected override string GetStringToRender(CheckboxState state)
        {
            char value = ' ';
            switch (state.Checked)
            {
                case TvCheckboxState.Checked:
                    value = 'X';
                    break;
                case TvCheckboxState.Partial:
                    value = '#';
                    break;
            }
            return $"[{value}]";
        }

    }
}
