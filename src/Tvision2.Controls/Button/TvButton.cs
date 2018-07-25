using System.Collections.Generic;
using System.Threading.Tasks;
using Tvision2.Controls.Styles;
using Tvision2.Core.Components.Behaviors;
using Tvision2.Core.Render;

namespace Tvision2.Controls.Button
{
    public class TvButton : TvControl<ButtonState>
    {
        public ICommand<ButtonState> OnClick { get; set; }

        public TvButton(ISkin skin, IViewport boxModel, ButtonState data) : base(skin, boxModel, data)
        {
        }


        protected override IEnumerable<ITvBehavior<ButtonState>> GetEventedBehaviors()
        {
            yield return new ButtonBehavior(async () => await (OnClick?.Invoke(State) ?? Task.CompletedTask));
        }

        protected override void OnDraw(RenderContext<ButtonState> context)
        {
            var focused = Metadata.IsFocused;
            var style = focused ? CurrentStyles.GetStyle("focused") : CurrentStyles.GetStyle("");
            var value = $"[ {State.Text.ToString()} ]";
            context.DrawStringAt(value, new TvPoint(0, 0), style.ForeColor, style.BackColor);
        }
    }
}
