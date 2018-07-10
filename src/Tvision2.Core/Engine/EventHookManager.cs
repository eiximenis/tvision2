using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tvision2.Core.Hooks;
using Tvision2.Events;

namespace Tvision2.Core.Engine
{
    class EventHookManager : IEventHookManager
    {
        private readonly List<IEventHook> _hooks;
        private readonly HookContext _context;

        public EventHookManager(IEnumerable<Type> hookTypes, HookContext context, IServiceProvider serviceProvider)
        {

            _hooks = hookTypes.Select(ht => serviceProvider.GetService(ht) as IEventHook).ToList();
            _context = context;
        }

        public void ProcessEvents(TvConsoleEvents evts)
        {
            if (evts.HasEvents)
            {
                foreach (var hook in _hooks)
                {
                    hook.ProcessEvents(evts, _context);
                }
            }
        }
    }
}
