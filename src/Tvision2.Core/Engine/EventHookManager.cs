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
        private readonly List<Action> _postUpdateActions;

        public EventHookManager(IEnumerable<Type> hookTypes, IEnumerable<Action> afterUpdates, HookContext context, IServiceProvider serviceProvider)
        {
            _hooks = new List<IEventHook>();

            foreach (var ht in hookTypes)
            {
                _hooks.Add(serviceProvider.GetService(ht) as IEventHook);
            }

            _context = context;
            _postUpdateActions = afterUpdates.ToList();
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

        public void ProcessAfterUpdateActions()
        {
            foreach (var action in _postUpdateActions)
            {
                action();
            }
        }
    }
}
