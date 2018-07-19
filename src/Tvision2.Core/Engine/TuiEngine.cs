using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Tvision2.Core.Hooks;
using Tvision2.Core.Render;
using Tvision2.Engine.Console;
using Tvision2.Core.Components.Draw;

namespace Tvision2.Core.Engine
{
    public class TuiEngine : ITuiEngine
    {
        
        private const long TIME_PER_FRAME = (int)((1.0f / 30.0f) * 1000) * TimeSpan.TicksPerMillisecond;
        private readonly EventPumper _eventPumper;
        private readonly Stopwatch _watcher;
        private readonly Tvision2Options _options;

        private readonly ComponentTree _ui;

        private VirtualConsole _currentConsole;
        public IConsoleDriver ConsoleDriver { get; }
        public IEventHookManager EventHookManager { get; }
        public IComponentTree UI => _ui;
        public IServiceProvider ServiceProvider { get; }

        public TuiEngine(Tvision2Options options, IServiceProvider serviceProvider)
        {
            ConsoleDriver = options.ConsoleDriver ?? throw new ArgumentException("No console driver defined");
            _ui = new ComponentTree(this);
            _eventPumper = new EventPumper(ConsoleDriver);
            _watcher = new Stopwatch();
            var hookContext = new HookContext(this);
            ServiceProvider = serviceProvider;
            _options = options;
            EventHookManager = new EventHookManager(options.HookTypes ?? Enumerable.Empty<Type>(), options.AfterUpdates ?? Enumerable.Empty<Action>(), hookContext, ServiceProvider);
        }

        public async Task Start(CancellationToken cancellationToken)
        {
            ConsoleDriver.Init();
            _currentConsole = new VirtualConsole();

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
                EventHookManager.ProcessEvents(evts);
                _ui.Update(evts);
                PerformDrawOperations(force: false);
                EventHookManager.ProcessAfterUpdateActions();
                // StateManager.DoDispatchAllActions();
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
                var updateActions = _currentConsole.UpdateActions;
                if (updateActions.Diffs.Any())
                {
                    VirtualConsoleRenderer.RenderToConsole(ConsoleDriver, updateActions.Diffs);
                }
                var (pendingCursorMovement, pos) = updateActions.CursorMovement;
                if (pendingCursorMovement)
                {
                    ConsoleDriver.SetCursorAt(pos.Left, pos.Top);
                }
                _currentConsole.NoDirty();
            }

            return flushed;
        }

    }
}
