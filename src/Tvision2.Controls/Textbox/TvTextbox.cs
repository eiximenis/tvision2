using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Controls.Styles;
using Tvision2.Core.Components;
using Tvision2.Core.Components.Behaviors;
using Tvision2.Core.Render;
using Tvision2.Controls.Extensions;

namespace Tvision2.Controls.Textbox
{
    public class TvTextbox : TvControl<TextboxState>
    {

        public static ITvControlCreationParametersBuilder<TextboxState> CreationParametersBuilder(Action<TextboxState> stateConfig = null)
        {
            if (stateConfig != null) {
                return TvControlCreationParametersBuilder.ForState<TextboxState>(() =>
                {
                var state = new TextboxState();
                stateConfig(state);
                return state;
                });
            }

            return TvControlCreationParametersBuilder.ForDefaultState<TextboxState>();
        }

        public TvTextbox(ITvControlCreationParametersBuilder<TextboxState> parameters) : this (parameters.Build()) { }

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
            var pairIdx = Metadata.IsFocused ? CurrentStyle.Focused : CurrentStyle.Standard;
            context.DrawChars(' ', context.Viewport.Bounds.Cols, new TvPoint(0, 0), pairIdx);
            if (!string.IsNullOrEmpty(State.Text))
            {
                context.DrawStringAt(State.Text, new TvPoint(0, 0), pairIdx);
            }
        }
    }
}
