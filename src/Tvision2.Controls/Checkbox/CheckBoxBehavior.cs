    using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Components.Behaviors;
using Tvision2.Events;

namespace Tvision2.Controls.Checkbox
{
    public class CheckBoxBehavior : KeyboardBehavior<CheckboxState>
    {
        protected override bool OnKeyDown(TvConsoleKeyboardEvent evt, BehaviorContext<CheckboxState> updateContext) => true;
        protected override bool OnKeyUp(TvConsoleKeyboardEvent evt, BehaviorContext<CheckboxState> updateContext)
        {
            var info = evt.AsConsoleKeyInfo();
            var state = updateContext.State;
            if (info.Key == ConsoleKey.Spacebar)
            {
                var checkState = state.Checked;
                switch (checkState)
                {
                    case TvCheckboxState.Checked:
                    case TvCheckboxState.Partial:
                        state.Checked = TvCheckboxState.Unchecked;
                        break;
                    default:
                        state.Checked = TvCheckboxState.Checked;
                        break;
                }
            }

            return true;
        }
    }
}
