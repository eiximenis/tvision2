﻿using System;
using Tvision2.Core.Render;
using Tvision2.Styles.Extensions;

namespace Tvision2.Controls.Label
{


    public class TvLabelParamsBuilder : TvControlCreationBuilder<TvLabel, LabelState, ITvLabelOptionsBuilder, ILabelOptions, TvLabelOptions> { }

    public class TvLabel : TvControl<LabelState, ILabelOptions>
    {
        public static ITvControlOptionsBuilder<TvLabel, LabelState, ITvLabelOptionsBuilder, ILabelOptions, TvLabelOptions> UseParams() => new TvLabelParamsBuilder();

        public TvLabel(TvControlCreationParameters<LabelState, ILabelOptions> parameters) : base(parameters)
        {
        }

        protected override void ConfigureMetadataOptions(TvControlMetadataOptions options)
        {
            options.FocusedWhen().Never();
        }

        protected override IViewport CalculateViewport(IViewport initialViewport)
        {
            return initialViewport.WithBounds(TvBounds.FromRowsAndCols(1, State.Text?.Length ?? 0));
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
