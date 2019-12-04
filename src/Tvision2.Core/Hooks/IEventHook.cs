using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Events;

namespace Tvision2.Core.Hooks
{
    public interface IEventHook
    {
        void ProcessEvents(ITvConsoleEvents events, HookContext context);
    }
}
