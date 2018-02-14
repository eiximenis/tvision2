using System;
using System.Collections.Generic;
using System.Text;
using TvConsole;
using Tvision2.Core.Components.Behaviors;
using Tvision2.Core.Engine;

namespace Tvision2.Controls.Button
{
    public class ButtonBehavior<T> : KeyboardBehavior<T>
    {
        private readonly Action _onClickAction;
        public ButtonBehavior(Action onClickAction) => _onClickAction = onClickAction;

        protected override bool OnKeyUp(TvConsoleKeyboardEvent evt, BehaviorContext<T> updateContext)
        {
            var properties = updateContext.State;

            /*
            bool isPressed = properties.GetPropertyAs<bool>("IsPressed");
            if (isPressed)
            {
                _onClickAction?.Invoke();
                properties.SetValues(new { IsPressed = false });
                return true;
            }

            */

            return false;
        }

        protected override bool OnKeyDown(TvConsoleKeyboardEvent evt, BehaviorContext<T> updateContext)
        {
            var properties = updateContext.State;

            /*

            bool isPressed = properties.GetPropertyAs<bool>("IsPressed");
            if (isPressed)
            {
                return false;
            }
            currentProperties.SetValues(new { IsPressed = true });
            */
            return true;
        }

    }
}