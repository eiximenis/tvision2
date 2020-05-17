using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Components.Behaviors;
using Tvision2.Core.Engine;
using Tvision2.Events;

namespace Tvision2.Controls.Button
{
    public class ButtonBehavior : KeyboardBehavior<ButtonState>
    {
        private readonly Action _onClickAction;
        public ButtonBehavior(Action onClickAction) => _onClickAction = onClickAction;

        protected override bool OnKeyUp(TvConsoleKeyboardEvent evt, BehaviorContext<ButtonState> updateContext)
        {
            var properties = updateContext.State;

            if (properties.IsPressed)
            {
                evt.Handle();
                _onClickAction?.Invoke();
                properties.IsPressed = false;
                return true;
            }

            return false;
        }

        protected override bool OnKeyDown(TvConsoleKeyboardEvent evt, BehaviorContext<ButtonState> updateContext)
        {
            if (evt.AsConsoleKeyInfo().Key != ConsoleKey.Spacebar)
            {
                return false;
            }
            evt.Handle();

            var properties = updateContext.State;
            if (properties.IsPressed)
            {
                return false;
            }
            properties.IsPressed = true;
            return true;
        }

    }
}