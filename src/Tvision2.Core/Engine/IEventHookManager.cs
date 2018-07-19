using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Hooks;
using Tvision2.Events;

namespace Tvision2.Core.Engine
{
    public interface IEventHookManager
    {
        void ProcessEvents(TvConsoleEvents evts);
        void ProcessAfterUpdateActions();
    }
}
