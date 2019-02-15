using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tvision2.Core;
using Tvision2.Core.Components.Behaviors;
using Tvision2.Core.Render;

namespace Tvision2.Controls.Button
{
    public class TvButton : TvControl<ButtonState>
    {
        private readonly ActionChain<ButtonState> _onClick;
        public IActionChain<ButtonState> OnClick => _onClick;

        public static ITvControlCreationParametersBuilder<ButtonState> CreationParametersBuilder(Action<ButtonState> stateCfg = null)
        {
            return TvControlCreationParametersBuilder.ForDefaultState<ButtonState>(stateCfg);
        }

        public TvButton(ITvControlCreationParametersBuilder<ButtonState> parameters) : this(parameters.Build()) { }
        public TvButton(TvControlCreationParameters<ButtonState> parameters) : base(parameters)
        {
            _onClick = new ActionChain<ButtonState>();
        }


        protected override IEnumerable<ITvBehavior<ButtonState>> GetEventedBehaviors()
        {
            yield return new ButtonBehavior(() => _onClick.Invoke(State) );
        }

        protected override void OnDraw(RenderContext<ButtonState> context)
        {
            var focused = Metadata.IsFocused;
            var pairIdx = focused ? CurrentStyle.Focused : CurrentStyle.Standard;
            var value = $"[ {State.Text.ToString()} ]";
            context.DrawStringAt(value, new TvPoint(0, 0), pairIdx);
        }
    }
}
