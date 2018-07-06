using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tvision2.Core.Hooks;
using Tvision2.Events;

namespace Tvision2.Controls.Hooks
{
    public class ChangeFocusEventHook : IEventHook
    {
        private readonly IControlsTree _ctree;

        public ChangeFocusEventHook(IControlsTree ctree) => _ctree = ctree;

        public void ProcessEvents(TvConsoleEvents events, HookContext context)
        {
            var tab = events.AcquireFirstKeyboard(ke => ke.IsKeyDown && ke.AsConsoleKeyInfo().Key == ConsoleKey.Tab);
            if (tab != null)
            {

                _ctree.MoveFocusToNext();
                _ctree.CurrentFocused().Control.OnFocus();
            }

        }
    }
}
