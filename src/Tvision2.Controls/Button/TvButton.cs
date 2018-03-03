using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Tvision2.Controls.Styles;
using Tvision2.Core.Components;
using Tvision2.Core.Components.Behaviors;
using Tvision2.Core.Render;

namespace Tvision2.Controls.Button
{
    public class TvButton : TvControl<ButtonState>
    {
        public ICommand<ButtonState> OnClick { get; set; }

        public TvButton(ISkin skin, IBoxModel boxModel, ButtonState data) : base(skin, boxModel, data)
        {
        }

        protected override void AddCustomElements(TvComponent<ButtonState> component)
        {
            component.AddBehavior(new ButtonBehavior(async () => await (OnClick?.Invoke(State) ?? Task.CompletedTask)), options =>
            {
                options.UseScheduler(BehaviorSchedule.OnEvents);
            });
        }

        protected override void OnDraw(RenderContext<ButtonState> context)
        {
            var focused = Style.ContainsClass("focused");
            var value = string.Format("{0}{1}{2}{3}{4}",
                new string(' ', Style.PaddingLeft),
                focused ? ">" : "",
                "[" + State.Text.ToString() + "]",
                focused ? "<" : "",
                new string(' ', Style.PaddingRight));

            context.Viewport.DrawStringAt(value, new TvPoint(0, 0), Style.ForeColor, Style.BackColor);
        }
    }
}
