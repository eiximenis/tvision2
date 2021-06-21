using System;
using System.Collections.Generic;
using Tvision2.Core;
using Tvision2.Core.Components.Behaviors;
using Tvision2.Core.Render;
using Tvision2.Styles.Extensions;

namespace Tvision2.Controls.Button
{

    public class TvButtonParamsBuilder : TvControlCreationBuilder<TvButton, ButtonState> { }

    public class TvButton : TvControl<ButtonState>
    {
        private readonly ActionChain<ButtonState> _onClick;
        public IActionChain<ButtonState> OnClick => _onClick;

        public static ITvControlOptionsBuilder<TvButton, ButtonState> UseParams() => new TvButtonParamsBuilder();

        public TvButton(TvControlCreationParameters<ButtonState> parameters) : base(parameters)
        {
            _onClick = new ActionChain<ButtonState>();
        }

        protected override IViewport CalculateViewport(IViewport initialViewport)
        {
            return initialViewport.WithBounds(TvBounds.FromRowsAndCols(1, State.Text.Length + 2));
        }


        protected override IEnumerable<ITvBehavior<ButtonState>> GetEventedBehaviors()
        {
            yield return new ButtonBehavior(() => _onClick.Invoke(State) );
            yield return new ButtonMouseBehavior(() => _onClick.Invoke(State));
        }

        protected override void OnDraw(RenderContext<ButtonState> context)
        {
            var focused = Metadata.IsFocused;
            var styleToUse = focused ? CurrentStyle.Active : CurrentStyle.Standard;
            context.Fill(styleToUse);
            var value = $"[{State.Text.ToString()}]";
            context.DrawStringAt(value, TvPoint.Zero, styleToUse);
        }

        public override string ToString()
        {
            return $"TvButton [{State.Text}] ({Name})";
        }
    }
}
