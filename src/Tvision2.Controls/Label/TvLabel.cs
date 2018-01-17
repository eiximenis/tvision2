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
            var definition = CreateDefinition();
            CreateComponent(definition);
        }

        protected override ComponentDefinition CreateDefinition()
        {
            var definition = base.CreateDefinition();
            definition.AddDrawer(new TextDrawer(options =>
            {
                options.PropertyName = "Text";
            }));

            return definition;
        }

    }
}
