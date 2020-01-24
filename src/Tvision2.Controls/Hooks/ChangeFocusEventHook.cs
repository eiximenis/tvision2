using System;
using Tvision2.Core.Hooks;
using Tvision2.Events;

namespace Tvision2.Controls.Hooks
{
    public class ChangeFocusEventHook : IEventHook
    {
        private readonly IControlsTree _ctree;

        public ChangeFocusEventHook(IControlsTree ctree) => _ctree = ctree;

        public void ProcessEvents(ITvConsoleEvents events, HookContext context)
        {
            var tab = events.AcquireFirstKeyboard(ke => ke.IsKeyDown && ke.AsConsoleKeyInfo().Key == ConsoleKey.Tab, autoHandle: true);
            if (tab != null)
            {
                _ctree.MoveFocusToNext();
            }
        }
    }
}
