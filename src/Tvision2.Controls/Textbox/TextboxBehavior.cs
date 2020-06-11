using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Components.Behaviors;
using Tvision2.Events;

namespace Tvision2.Controls.Textbox
{
    public class TextboxBehavior : KeyboardBehavior<TextboxState>
    {
        protected override bool OnKeyPress(TvConsoleKeyboardEvent evt, BehaviorContext<TextboxState> updateContext)
        {
            evt.Handle();
            updateContext.State.ProcessKey(evt.AsConsoleKeyInfo());
            return true;
        }
        
    }
}
