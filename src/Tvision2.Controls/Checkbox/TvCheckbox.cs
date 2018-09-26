using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Controls.Styles;
using Tvision2.Core.Components;
using Tvision2.Core.Components.Behaviors;
using Tvision2.Core.Render;

namespace Tvision2.Controls.Checkbox
{
    public class TvCheckbox : TvControl<CheckboxState>
    {
        private static TvPoint _focusOffset = new TvPoint(1, 0);
        public TvCheckbox(ISkin skin, IViewport boxModel, CheckboxState state) : base(skin, boxModel, state)
        {
        }

        protected override IEnumerable<ITvBehavior<CheckboxState>> GetEventedBehaviors()
        {
            yield return new CheckBoxBehavior();
        }

        protected override TvPoint CalculateFocusOffset() => _focusOffset;
        protected override void OnDraw(RenderContext<CheckboxState> context)
        {
            var pairIdx = Metadata.IsFocused ? CurrentStyle.Focused : CurrentStyle.Standard;
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

            context.DrawStringAt($"[{value}]", new TvPoint(0, 0), pairIdx);
        }

    }
}
