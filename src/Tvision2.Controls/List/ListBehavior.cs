using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Components.Behaviors;
using Tvision2.Events;

namespace Tvision2.Controls.List
{
    public class ListBehavior<T> : KeyboardBehavior<ListState<T>>
    {
        protected override bool OnKeyUp(TvConsoleKeyboardEvent evt, BehaviorContext<ListState<T>> updateContext) => true;
        protected override bool OnKeyDown(TvConsoleKeyboardEvent evt, BehaviorContext<ListState<T>> updateContext)
        {
            var info = evt.AsConsoleKeyInfo();
            if (info.Key == ConsoleKey.DownArrow && updateContext.State.CanScroll)
            {
                if (updateContext.State.NeedsToScroll)
                {
                    updateContext.State.ScrollDown(1);
                }
                updateContext.State.SelectedIndex++;
            }
            else if (info.Key == ConsoleKey.UpArrow)
            {
                updateContext.State.SelectedIndex--;
            }

            return true;
        }
    }
}
