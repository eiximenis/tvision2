using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Components.Render;

namespace Tvision2.Controls.Button
{
    public class TvButton : TvControl<ButtonState>
    {
        public TvButton(ButtonState state) : base(state)
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
