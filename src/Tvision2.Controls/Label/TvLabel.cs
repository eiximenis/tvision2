using System;
using Tvision2.Core.Render;
using Tvision2.Styles.Extensions;

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
        }

        protected override void ConfigureMetadataOptions(TvControlMetadataOptions options)
        {
            options.IsFocused().Never();
        }

        protected override void OnDraw(RenderContext<LabelState> context)
        {
            var state = context.State;
            var currentcols = context.Viewport.Bounds.Cols;
            var focused = Metadata.IsFocused;
            var pairIdx = focused ? CurrentStyle.Active : CurrentStyle.Standard;
            var value = state.Text.ToString() ?? "";
            context.Fill(pairIdx);
            context.DrawStringAt(value, TvPoint.Zero, pairIdx);
        }
    }
}
