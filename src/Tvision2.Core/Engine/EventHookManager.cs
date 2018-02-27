using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Hooks;
using Tvision2.Events;

namespace Tvision2.Core.Engine
{
    class EventHookManager : IEventHookManager
    {
        private readonly List<IEventHook> _hooks;
        private readonly HookContext _context;

        public EventHookManager(IEnumerable<IEventHook> hooks, HookContext context)
        {
            _hooks = new List<IEventHook>(hooks);
            _context = context;
        }

        public void ProcessEvents(TvConsoleEvents evts)
        {
            foreach (var hook in _hooks)
            {
                hook.ProcessEvents(evts, _context);
            }
        }
    }
}
