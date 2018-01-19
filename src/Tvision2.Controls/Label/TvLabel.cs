using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Controls.Behavior;
using Tvision2.Core.Components;
using Tvision2.Core.Components.Props;
using Tvision2.Core.Components.Render;

namespace Tvision2.Controls.Label
{
    public class TvLabel : TvControl<LabelState>
    {

        public TvLabel(LabelState state) : base(state)
        {
        }

        protected override void AddComponentElements(TvComponent cmp)
        {
            cmp.AddDrawer(new TextDrawer(options =>
            {
                options.PropertyName = "Text";
            }));
        }


    }
}
