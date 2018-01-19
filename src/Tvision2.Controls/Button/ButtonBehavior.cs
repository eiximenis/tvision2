using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Components.Behaviors;
using Tvision2.Core.Components.Props;
using Tvision2.Core.Engine;

namespace Tvision2.Controls.Button
{
    public class ButtonBehavior : KeyboardBehavior
    {
        protected override IPropertyBag OnKeyDown(KeyEvent evt, BehaviorContext updateContext, IPropertyBag currentProperties)
        {
            bool isPressed = currentProperties.GetPropertyAs<bool>("IsPressed");
            if (isPressed) return null;

            return currentProperties.SetValues(new { IsPressed = true });
        }
    }
}