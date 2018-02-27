using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Events;

namespace Tvision2.Core.Hooks
{
    public abstract class KeyboardHook : IEventHook
    {
        public void ProcessEvents(TvConsoleEvents events, HookContext context) => ProcessEvents(events.KeyboardEvents, context);

        protected abstract void ProcessEvents(IEnumerable<TvConsoleKeyboardEvent> keyboardEvents, HookContext context);
    }
}
