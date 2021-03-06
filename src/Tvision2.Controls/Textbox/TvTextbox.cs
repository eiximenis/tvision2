﻿using System;
using System.Collections.Generic;
using Tvision2.Core.Components.Behaviors;
using Tvision2.Core.Render;
using Tvision2.Styles.Extensions;

namespace Tvision2.Controls.Textbox
{

    class TvTextboxParamsBuilder : TvControlCreationBuilder<TvTextbox, TextboxState> { }
    public class TvTextbox : TvControl<TextboxState>
    {

        public static ITvControlOptionsBuilder<TvTextbox, TextboxState> UseParams() => new TvTextboxParamsBuilder();

        public TvTextbox(TvControlCreationParameters<TextboxState> parameters) : base(parameters)
        {
            RequestCursorManagement((ctx, state) => ctx.SetCursorAt(state.CaretPos, 0));
        }

        protected override IEnumerable<ITvBehavior<TextboxState>> GetEventedBehaviors()
        {
            yield return new TextboxBehavior(); 
        }

        protected override void OnDraw(RenderContext<TextboxState> context)
        {
            var pairIdx = Metadata.IsFocused ? CurrentStyle.Active : CurrentStyle.Standard;
            context.DrawChars(' ', context.Viewport.Bounds.Cols, TvPoint.Zero, pairIdx);
            if (!string.IsNullOrEmpty(State.Text))
            {
                context.DrawStringAt(State.Text, TvPoint.Zero, pairIdx);
            }
        }
    }
}
