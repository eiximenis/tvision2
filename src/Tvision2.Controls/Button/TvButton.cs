using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Tvision2.Core.Components;
using Tvision2.Core.Components.Behaviors;
using Tvision2.Core.Components.Render;

namespace Tvision2.Controls.Button
{
    public class TvButton : TvControl<ButtonState>
    {
        public ICommand<ButtonState> OnClick { get; set; }

        public TvButton(ButtonState state) : base(state)
        {    
        }

        protected override void AddComponentElements(TvComponent<ButtonState> cmp)
        {

            cmp.AddBehavior(new ButtonBehavior<ButtonState>(async () => await OnClick.Invoke(State)), options =>
            {
                options.UseScheduler(BehaviorSchedule.OnEvents);
            });
            cmp.AddDrawer(new TextDrawer<ButtonState>(options =>
            {
                options.PropertyName = "Text";
            }));
        }


    }
}
