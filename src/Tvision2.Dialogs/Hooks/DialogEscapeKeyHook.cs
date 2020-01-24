using System;
using Tvision2.Core.Hooks;
using Tvision2.Events;

namespace Tvision2.Dialogs.Hooks
{
    public class DialogEscapeKeyHook : IEventHook
    {
        private readonly IDialogManager _dialogManager;

        public DialogEscapeKeyHook(IDialogManager dialogManager) => _dialogManager = dialogManager;

        public void ProcessEvents(ITvConsoleEvents events, HookContext context)
        {
            var esc = events.AcquireFirstKeyboard(ke => ke.IsKeyDown && ke.AsConsoleKeyInfo().Key == ConsoleKey.Escape, autoHandle: true);
            if (esc != null && _dialogManager.DialogShown != null)
            {
                _dialogManager.CloseDialog();
            }
        }
    }
}
