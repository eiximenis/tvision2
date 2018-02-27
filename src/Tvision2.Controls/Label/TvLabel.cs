using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Controls.Behavior;
using Tvision2.Controls.Draw;
using Tvision2.Core.Components;
using Tvision2.Core.Components.Render;

namespace Tvision2.Controls.Label
{
    public class TvLabel : TvControl<LabelState>
    {

        public TvLabel(LabelState state) : base(state, state)
        {
        }

        protected override void AddComponentElements(TvComponent<LabelState> cmp)
        {
            cmp.AddDrawer(new ControlTextDrawer<LabelState>(lstate => lstate.Text));
        }


    }
}
