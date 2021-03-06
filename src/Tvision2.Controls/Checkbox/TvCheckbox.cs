﻿using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Components;
using Tvision2.Core.Components.Behaviors;
using Tvision2.Core.Render;

namespace Tvision2.Controls.Checkbox
{

    public class TvCheckboxParamsBuilder : TvControlCreationBuilder<TvCheckbox, CheckboxState> { }
    public class TvCheckbox : TvControl<CheckboxState>
    {

        public static ITvControlOptionsBuilder<TvCheckbox, CheckboxState> UseParams() => new TvCheckboxParamsBuilder();

        public TvCheckbox(TvControlCreationParameters<CheckboxState> parameters) : base(parameters)
        {
            RequestCursorManagement((ctx, _) => ctx.SetCursorAt(1,0));
        }

        protected override IEnumerable<ITvBehavior<CheckboxState>> GetEventedBehaviors()
        {
            yield return new CheckBoxBehavior();
        }

        protected override void OnDraw(RenderContext<CheckboxState> context)
        {
            var pairIdx = Metadata.IsFocused ? CurrentStyle.Active : CurrentStyle.Standard;
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

            context.DrawStringAt($"[{value}]", TvPoint.Zero, pairIdx.ToCharacterAttribute(TvPoint.Zero, context.Viewport.Bounds));
        }

    }
}
