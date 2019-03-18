using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Controls.Behavior;
using Tvision2.Controls.Styles;
using Tvision2.Core.Components;
using Tvision2.Core.Render;

namespace Tvision2.Controls.Label
{
    public class TvLabel : TvControl<LabelState>
    {

        public static ITvControlCreationParametersBuilder<LabelState> CreationParametersBuilder(Action<LabelState> stateConfig = null)
        {
            if (stateConfig != null)
            {
                return TvControlCreationParametersBuilder.ForState<LabelState>(() =>
                {
                    var state = new LabelState();
                    stateConfig(state);
                    return state;
                });
            }

            return TvControlCreationParametersBuilder.ForDefaultState<LabelState>();
        }
        public TvLabel(ITvControlCreationParametersBuilder<LabelState> parameters) : this(parameters.Build()) { }
        public TvLabel(TvControlCreationParameters<LabelState> parameters) : base(parameters)
        {
            Metadata.CanFocus = false;
        }

        protected override void OnDraw(RenderContext<LabelState> context)
        {
            var state = context.State;
            var currentcols = context.Viewport.Bounds.Cols;
            var focused = Metadata.IsFocused;
            var pairIdx = focused ? CurrentStyle.Focused : CurrentStyle.Standard;
            var value = state.Text.ToString() ?? "";
            context.Fill(pairIdx);
            context.DrawStringAt(value, new TvPoint(0, 0), pairIdx);
        }
    }
}
