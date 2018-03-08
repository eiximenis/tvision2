using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Controls.Styles;
using Tvision2.Core.Components;
using Tvision2.Core.Render;

namespace Tvision2.Controls.Checkbox
{
    public class TvCheckbox : TvControl<CheckboxState>
    {
        public TvCheckbox(ISkin skin, IBoxModel boxModel, CheckboxState state) : base(skin, boxModel, state)
        {
        }

        protected override void OnDraw(RenderContext<CheckboxState> context)
        {
            var state = context.State;
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

            context.DrawStringAt($"[{value}]", new TvPoint(0, 0), Style.ForeColor, Style.BackColor);
        }

    }
}
