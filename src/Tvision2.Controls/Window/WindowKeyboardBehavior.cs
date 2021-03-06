﻿using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Components.Behaviors;
using Tvision2.Core.Render;
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

        protected override bool OnKeyPress(TvConsoleKeyboardEvent evt, BehaviorContext<WindowState> updateContext)
        {
            var keyInfo = evt.AsConsoleKeyInfo();
            if (keyInfo.Modifiers == ConsoleModifiers.Control && keyInfo.Key == ConsoleKey.Spacebar)
            {
                evt.Handle();
                _moving = true;
                return false;
            }

            if (_moving)
            {
                switch (keyInfo.Key)
                {
                    /* TODO: UpdateViewport is not supported right now, as context is ref readonly struct
                    case ConsoleKey.RightArrow:
                        updateContext.UpdateViewport(updateContext.Viewport.Translate(TvPoint.FromXY(1, 0)));
                        evt.Handle();
                        return true;
                    case ConsoleKey.LeftArrow:
                        updateContext.UpdateViewport(updateContext.Viewport.Translate(TvPoint.FromXY(-1, 0)));
                        evt.Handle();
                        return true;
                    case ConsoleKey.UpArrow:
                    evt.Handle();
                        updateContext.UpdateViewport(updateContext.Viewport.Translate(TvPoint.FromXY(0, -1)));
                        return true;
                    case ConsoleKey.DownArrow:
                        updateContext.UpdateViewport(updateContext.Viewport.Translate(TvPoint.FromXY(0, 1)));
                        evt.Handle();
                        return true;
                    case ConsoleKey.Escape:
                        _moving = false;
                        evt.Handle();
                        return false;
                    */
                }
            }
            else
            {
                if (keyInfo.Key == ConsoleKey.Escape)
                {
                    evt.Handle();
                    updateContext.State.RequestClose();
                }
            }
            return false;
        }
    }
}
