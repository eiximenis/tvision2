using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Components.Behaviors;
using Tvision2.Events;

namespace Tvision2.Controls.Window
{
    public class WindowKeyboardBehavior : KeyboardBehavior<WindowState>
    {

        private bool _moving;

        public WindowKeyboardBehavior()
        {
            _moving = false;
        }

        protected override bool OnKeyUp(TvConsoleKeyboardEvent evt, BehaviorContext<WindowState> updateContext) => true;
        protected override bool OnKeyDown(TvConsoleKeyboardEvent evt, BehaviorContext<WindowState> updateContext)
        {
            var keyInfo = evt.AsConsoleKeyInfo();
            if (keyInfo.Modifiers == ConsoleModifiers.Control && keyInfo.Key == ConsoleKey.Spacebar)
            {
                _moving = true;
                return false;
            }

            if (_moving)
            {
                switch (keyInfo.Key)
                {
                    case ConsoleKey.RightArrow:
                        updateContext.UpdateViewport(updateContext.Viewport.Translate(new Core.Render.TvPoint(1, 0)));
                        return true;
                }
            }
            return false;
        }
    }
}
