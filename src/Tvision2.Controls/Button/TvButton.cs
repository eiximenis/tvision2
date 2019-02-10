using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tvision2.Core.Components.Behaviors;
using Tvision2.Core.Render;

namespace Tvision2.Controls.Button
{
    public class TvButton : TvControl<ButtonState>
    {
        public ICommand<ButtonState> OnClick { get; set; }

        public static ITvControlCreationParametersBuilder<ButtonState> CreationParametersBuilder(Action<ButtonState> stateCfg = null)
        {
            return TvControlCreationParametersBuilder.ForDefaultState<ButtonState>(stateCfg);
        }

        public TvButton(ITvControlCreationParametersBuilder<ButtonState> parameters) : this(parameters.Build()) { }
        public TvButton(TvControlCreationParameters<ButtonState> parameters) : base(parameters) { }


        protected override IEnumerable<ITvBehavior<ButtonState>> GetEventedBehaviors()
        {
            yield return new ButtonBehavior(async () => await (OnClick?.Invoke(State) ?? Task.CompletedTask));
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
