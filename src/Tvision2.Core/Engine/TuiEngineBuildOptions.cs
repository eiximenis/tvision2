using System;
using System.Collections.Generic;
using Tvision2.Core.Hooks;
using Tvision2.Core.Render;
using Tvision2.Engine.Console;

namespace Tvision2.Core.Engine
{
    public class TuiEngineBuildOptions
    {
        private readonly Dictionary<Type, object> _customItems;
        private readonly List<IEventHook> _hooks;

        public IEnumerable<IEventHook> Hooks => _hooks;
        public IConsoleDriver ConsoleDriver { get; internal set; }

        public IEnumerable<KeyValuePair<Type, object>> AdditionalValues => _customItems;

        internal void AddHook(IEventHook hook)
        {
            if (!_hooks.Contains(hook))
            {
                _hooks.Add(hook);
            }
        }


        internal void SetCustomItem<T>(T item)
        {
            _customItems.Add(typeof(T), item);
        }

        internal T GetCustomItem<T>() => _customItems.TryGetValue(typeof(T), out object value) ? (T)value : default(T);

        public TuiEngineBuildOptions()
        {
            _customItems = new Dictionary<Type, object>();
            _hooks = new List<IEventHook>();
        }


    }
}