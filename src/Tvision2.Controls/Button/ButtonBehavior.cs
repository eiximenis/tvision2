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

        protected override bool OnKeyPress(TvConsoleKeyboardEvent evt, BehaviorContext<ButtonState> updateContext)
        {
            if (evt.AsConsoleKeyInfo().Key != ConsoleKey.Spacebar)
            {
                return false;
            }

            evt.Handle();
            _onClickAction?.Invoke();
            return true;

        }

    }
}