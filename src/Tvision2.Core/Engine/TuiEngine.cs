using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Tvision2.Core.Hooks;
using Tvision2.Core.Render;
using Tvision2.Engine.Console;

namespace Tvision2.Core.Engine
{
    public class TuiEngine : ICustomItemsProvider
    {
        private const long TIME_PER_FRAME = (int)((1.0f / 30.0f) * 1000) * TimeSpan.TicksPerMillisecond;
        private readonly EventPumper _eventPumper;
        private readonly Stopwatch _watcher;

        private VirtualConsole _currentConsole;

        private readonly IDictionary<Type, object> _additionalItems;
        private readonly IConsoleDriver _consoleDriver;

        public IConsoleDriver ConsoleDriver => _consoleDriver;

        public IEventHookManager EventHookManager { get; }

        public ComponentTree UI { get; }

        internal TuiEngine(TuiEngineBuildOptions options)
        {
            _consoleDriver = options.ConsoleDriver ?? throw new ArgumentException("No console driver defined");
            _additionalItems = new Dictionary<Type, object>();
            UI = new ComponentTree(this);
            _additionalItems.Add(typeof(IComponentTree), UI);
            AddAdditionalItems(options.AdditionalValues);
            _eventPumper = new EventPumper(_consoleDriver);
            _watcher = new Stopwatch();
            var hookContext = new HookContext(this);
            EventHookManager = new EventHookManager(options.Hooks ?? Enumerable.Empty<IEventHook>(), hookContext);
        }

        private void AddAdditionalItems(IEnumerable<KeyValuePair<Type, object>> additionalItems)
        {
            foreach (var kvp in additionalItems)
            {
                var item = kvp.Value;
                _additionalItems.Add(kvp.Key, item);
            }
        }

        public TItem GetCustomItem<TItem>()
        {
            if (_additionalItems.TryGetValue(typeof(TItem), out object value))
            {
                return (TItem)value;
            }

            return default(TItem);
        }


        public async Task<int> Start()
        {
            _consoleDriver.Init();
            _currentConsole = new VirtualConsole();
            PerformDrawOperations(force: true);
            var end = false;
            do
            {
                _watcher.Start();
                var evts = _eventPumper.ReadEvents();
                EventHookManager.ProcessEvents(evts);
                UI.Update(evts);
                PerformDrawOperations(force: false);
                // StateManager.DoDispatchAllActions();
                _watcher.Stop();
                var ellapsed = (int)_watcher.ElapsedTicks;
                if (ellapsed < TIME_PER_FRAME)
                {
                    await Task.Delay(TimeSpan.FromTicks(TIME_PER_FRAME - ellapsed));
                }
                _watcher.Reset();

            } while (!end);

            return 0;
        }


        private void PerformDrawOperations(bool force)
        {
            UI.Draw(_currentConsole, force);
            FlushToRealConsole();
        }


        private bool FlushToRealConsole()
        {
            var flushed = false;

            if (_currentConsole.IsDirty)
            {
                flushed = true;
                var diffs = _currentConsole.GetDiffs();
                if (diffs.Any())
                {
                    VirtualConsoleRenderer.RenderToConsole(_consoleDriver, diffs);
                }
                _currentConsole.NoDirty();
            }

            return flushed;
        }

    }
}
