using System;
using System.Collections.Generic;
using Tvision2.Core.Hooks;
using Tvision2.Core.Render;
using Tvision2.Engine.Console;

namespace Tvision2.Core.Engine
{
    public class TuiEngineBuilder
    {
        private readonly TuiEngineBuildOptions _options;
        private readonly List<Action<TuiEngine>> _afterCreateActions;
        private readonly List<Action<TuiEngineBuilder>> _beforeCreateActions;

        public TuiEngineBuilder()
        {
            _options = new TuiEngineBuildOptions();
            _afterCreateActions = new List<Action<TuiEngine>>();
            _beforeCreateActions = new List<Action<TuiEngineBuilder>>();
        }

        public void AfterCreateInvoke(Action<TuiEngine> action)
        {
            _afterCreateActions.Add(action);
        }

        public void BeforeCreateInvoke(Action<TuiEngineBuilder> action)
        {
            _beforeCreateActions.Add(action);
        }


        public TuiEngine Build()
        {
            foreach (var action in _beforeCreateActions)
            {
                action.Invoke(this);
            }
            var engine = new TuiEngine(_options);
            foreach (var action in _afterCreateActions)
            {
                action.Invoke(engine);
            }
            return engine;
        }

        public TuiEngineBuilder AddEventHook(IEventHook hook)
        {
            _options.AddHook(hook);
            return this;
        }

        public TuiEngineBuilder UseConsoleDriver(IConsoleDriver driverToUse)
        {
            _options.ConsoleDriver = driverToUse;
            return this;
        }

        public void SetCustomItem<T>(T item)
        {
            _options.SetCustomItem(item);
        }

        public T GetCustomItem<T>() => _options.GetCustomItem<T>();
    }
}
