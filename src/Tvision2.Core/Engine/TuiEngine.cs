using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Tvision2.Core.Hooks;
using Tvision2.Core.Render;
using Tvision2.Engine.Console;

namespace Tvision2.Core.Engine
{
    public class TuiEngine : ITuiEngine
    {

        private const long TIME_PER_FRAME = (int)((1.0f / 30.0f) * 1000) * TimeSpan.TicksPerMillisecond;
        private readonly EventPumper _eventPumper;
        private readonly Stopwatch _watcher;
        private readonly Tvision2Options _options;
        private EventHookManager _eventHookManager;

        private readonly ComponentTree _ui;

        private VirtualConsole _currentConsole;
        public IConsoleDriver ConsoleDriver { get; }
        public IEventHookManager EventHookManager  => _eventHookManager;
        public IComponentTree UI => _ui;
        public IServiceProvider ServiceProvider { get; }

        public TuiEngine(Tvision2Options options, IServiceProvider serviceProvider)
        {
            ConsoleDriver = options.ConsoleDriver ?? throw new ArgumentException("No console driver defined");
            _ui = new ComponentTree(this);
            _eventPumper = new EventPumper(ConsoleDriver);
            _watcher = new Stopwatch();
            ServiceProvider = serviceProvider;
            _options = options;
        }

        public async Task Start(CancellationToken cancellationToken)
        {
            ConsoleDriver.Init();
            var bounds = ConsoleDriver.ConsoleBounds;
            _currentConsole = new VirtualConsole(bounds.Rows, bounds.Cols);
            var hookContext = new HookContext(this);
            _eventHookManager = new EventHookManager(_options.HookTypes ?? Enumerable.Empty<Type>(), _options.AfterUpdates ?? Enumerable.Empty<Action>(), hookContext, ServiceProvider);
            foreach (var afterCreateTask in _options.AfterCreateInvokers)
            {
                afterCreateTask.Invoke(this, ServiceProvider);
            }

            if (_options.StartupFunc != null)
            {
                await _options.StartupFunc(ServiceProvider, this);
            }

            var startup = ServiceProvider.GetService<ITvisionAppStartup>();

            if (startup != null)
            {
                await startup.Startup(this);
            }

            PerformDrawOperations(force: true);
            while (!cancellationToken.IsCancellationRequested)
            {
                _watcher.Start();
                var evts = _eventPumper.ReadEvents();

                if (evts.HasWindowEvent)
                {
                    ConsoleDriver.ProcessWindowEvent(evts.WindowEvent);
                    _currentConsole.Resize(evts.WindowEvent.NewRows, evts.WindowEvent.NewColumns);
                }

                _eventHookManager.ProcessEvents(evts);
                _ui.Update(evts);
                PerformDrawOperations(force: false);
                _eventHookManager.ProcessAfterUpdateActions();
                // TODO: At this point we still have all events in evts, but:
                //  1. No clue of which events had been processed (should annotate that)
                //  2. Don't do anything with remaining events. A list of pluggable "remaining events handler" could be implemented
                _watcher.Stop();
                var ellapsed = (int)_watcher.ElapsedTicks;
                if (ellapsed < TIME_PER_FRAME)
                {
                    await Task.Delay(TimeSpan.FromTicks(TIME_PER_FRAME - ellapsed), cancellationToken);
                }
                _watcher.Reset();
            }
        }

        private void PerformDrawOperations(bool force)
        {
            _ui.Draw(_currentConsole, force);
            FlushToRealConsole();
        }


        private bool FlushToRealConsole()
        {
            var flushed = false;

            if (_currentConsole.IsDirty)
            {
                flushed = true;
                _currentConsole.Flush(ConsoleDriver);
            }

            return flushed;
        }

    }
}
