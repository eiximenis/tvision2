﻿using System;
using System.Collections.Generic;
using System.Text;
using TvConsole;
using Tvision2.Core.Components.Behaviors;
using Tvision2.Core.Engine;

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
                 _onClickAction?.Invoke();
                properties.IsPressed = false;
                return true;
            }

            return false;
        }

        protected override bool OnKeyDown(TvConsoleKeyboardEvent evt, BehaviorContext<ButtonState> updateContext)
        {
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