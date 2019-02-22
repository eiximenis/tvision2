using System;
using System.Collections.Generic;
using System.Linq;
using Tvision2.Core.Hooks;
using Tvision2.Events;

namespace Tvision2.Core.Engine
{
    class EventHookManager : IEventHookManager
    {
        private readonly List<(Guid Id, IEventHook hook)> _hooks;
        private readonly HookContext _context;
        private readonly List<Action> _postUpdateActions;

        public EventHookManager(IEnumerable<Type> hookTypes, IEnumerable<Action> afterUpdates, HookContext context, IServiceProvider serviceProvider)
        {
            _hooks = new List<(Guid, IEventHook)>();

            foreach (var ht in hookTypes)
            {
                _hooks.Add((Guid.Empty, serviceProvider.GetService(ht) as IEventHook));
            }

            _context = context;
            _postUpdateActions = afterUpdates.ToList();
        }

        public void ProcessEvents(TvConsoleEvents evts)
        {
            if (evts.HasEvents)
            {
                foreach (var entry in _hooks)
                {
                    entry.hook.ProcessEvents(evts, _context);
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

        public Guid AddHook(IEventHook hookInstance)
        {
            var guid = Guid.NewGuid();
            _hooks.Add((guid, hookInstance));
            return guid;
        }

        public bool RemoveHook(Guid id)
        {
            var removed = false;
            if (id != Guid.Empty)
            {
                for (var idx = 0; idx < _hooks.Count; idx++)
                {
                    var entry = _hooks[idx];
                    if (entry.Id == id)
                    {
                        _hooks.RemoveAt(idx);
                        removed = true;
                        break;
                    }
                }
            }
            return removed;
        }

    }
}
