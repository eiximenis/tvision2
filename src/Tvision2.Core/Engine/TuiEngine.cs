﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Tvision2.Core.Render;

namespace Tvision2.Core.Engine
{
    public class TuiEngine
    {
        private const long TIME_PER_FRAME = (int)((1.0f / 30.0f) * 1000) * TimeSpan.TicksPerMillisecond;
        private readonly EventPumper _eventPumper;
        private readonly Stopwatch _watcher;

        private VirtualConsole _currentConsole;
        private VirtualConsole _previousConsole;

        private readonly IDictionary<string, object> _additionalItems;

        public ComponentTree UI { get; }
        internal TuiEngine(IDictionary<string, object> additionalItems)
        {
            _additionalItems = additionalItems ?? new Dictionary<string, object>();
            UI = new ComponentTree();
            _eventPumper = new EventPumper();
            _watcher = new Stopwatch();
        }

        public TItem GetCustomItem<TItem>(string key)
        {
            if (_additionalItems.TryGetValue(key, out object value))
            {
                return (TItem)value;
            }

            return default(TItem);
        }


        public async Task<int> Start()
        {
            _previousConsole = new VirtualConsole();
            _currentConsole = new VirtualConsole();
            PerformDrawOperations(force: true);
            var end = false;
            do
            {
                _watcher.Start();
                var evts = _eventPumper.ReadEvents();
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
            SyncConsoles();
        }


        private void FlushToRealConsole()
        {
            var diffs = _currentConsole.DiffWith(_previousConsole);
            if (diffs.Length > 0)
            {
                VirtualConsoleRenderer.RenderToConsole(diffs);
            }
        }

        private void SyncConsoles()
        {
            _previousConsole.FillWith(_currentConsole);
        }
    }
}
