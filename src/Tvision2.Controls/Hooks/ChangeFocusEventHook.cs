using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tvision2.Core.Hooks;
using Tvision2.Events;

namespace Tvision2.Controls.Hooks
{
    public class ChangeFocusEventHook : KeyboardHook
    {
        protected override void ProcessEvents(IEnumerable<TvConsoleKeyboardEvent> keyboardEvents, HookContext context)
        {
            var tab = keyboardEvents.FirstOrDefault(ke => !ke.IsKeyDown && ke.AsConsoleKeyInfo().Key == ConsoleKey.Tab);
            if (tab != null)
            {
                var controls = context.ItemsProvider.GetCustomItem<IControlsTree>();
                controls.MoveFocusToNext();
            }

        }
    }
}
