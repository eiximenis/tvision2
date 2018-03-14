using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Components.Behaviors;
using Tvision2.Events;

namespace Tvision2.Controls.Textbox
{
    public class TextboxBehavior : KeyboardBehavior<TextboxState>
    {
        protected override bool OnKeyDown(TvConsoleKeyboardEvent evt, BehaviorContext<TextboxState> updateContext)
        {
            return true;
        }

        protected override bool OnKeyUp(TvConsoleKeyboardEvent evt, BehaviorContext<TextboxState> updateContext)
        {
            var character = evt.Character;
            updateContext.State.ProcessKey(evt.AsConsoleKeyInfo());
            return true;
        }
    }
}
