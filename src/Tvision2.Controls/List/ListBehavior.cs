using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Components.Behaviors;
using Tvision2.Events;

namespace Tvision2.Controls.List
{
    public class ListBehavior<T> : KeyboardBehavior<ListState<T>>
    {

        private readonly Action _itemClickedAction;
        public ListBehavior(Action itemClickedAction)
        {
            _itemClickedAction = itemClickedAction;
        }

        protected override bool OnKeyPress(TvConsoleKeyboardEvent evt, BehaviorContext<ListState<T>> updateContext)
        {
            var info = evt.AsConsoleKeyInfo();
            if (info.Key == ConsoleKey.DownArrow && updateContext.State.CanScrollDown)
            {
                if (updateContext.State.NeedsToScrollDown)
                {
                    updateContext.State.ScrollDown(1);
                }
                updateContext.State.SelectedIndex++;
                return true;
            }

            if (info.Key == ConsoleKey.UpArrow && updateContext.State.CanScrollUp)
            {
                if (updateContext.State.NeedsToScrollUp)
                {
                    updateContext.State.ScrollUp(1);
                }
                updateContext.State.SelectedIndex--;
                return true;
            }

            if (info.Key == ConsoleKey.Enter)
            {
                _itemClickedAction?.Invoke();
                return true;
            }

            return false;
        }
    }
}
