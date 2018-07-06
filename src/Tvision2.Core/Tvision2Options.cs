using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tvision2.Core.Engine;
using Tvision2.Core.Hooks;
using Tvision2.Engine.Console;

namespace Tvision2.Core
{
    public interface ITvision2Options
    {
        ITvision2Options UseConsoleDriver(IConsoleDriver driverToUse);
        ITvision2Options UseStartup(Func<IServiceProvider, ITuiEngine, Task> startupAction);
        ITvision2Options UseStartup<TStartup>() where TStartup : ITvisionAppStartup;
        ITvision2Options AfterCreateInvoke(Action<ITuiEngine, IServiceProvider> invoker);

    }
    public class Tvision2Options : ITvision2Options
    {
        private readonly List<Type> _hookTypes;
        private readonly List<Action<ITuiEngine, IServiceProvider>> _afterCreateInvokers;

        public const string PropertyKey = "__Tvision2Options";

        public IEnumerable<Type> HookTypes => _hookTypes;
        public IConsoleDriver ConsoleDriver { get; private set; }

        public Func<IServiceProvider, ITuiEngine, Task> StartupFunc{ get; private set; }
        public Type StartupType { get; private set; }

        public Tvision2Options()
        {
            _hookTypes = new List<Type>();
            _afterCreateInvokers = new List<Action<ITuiEngine, IServiceProvider>>();

        }

        ITvision2Options ITvision2Options.AfterCreateInvoke(Action<ITuiEngine, IServiceProvider> invoker)
        {
            _afterCreateInvokers.Add(invoker);
            return this;
        }

        ITvision2Options ITvision2Options.UseConsoleDriver(IConsoleDriver driverToUse)
        {
            ConsoleDriver = driverToUse;
            return this;
        }

        ITvision2Options ITvision2Options.UseStartup(Func<IServiceProvider, ITuiEngine, Task> startupAction)
        {
            StartupFunc = startupAction;
            return this;
        }

        ITvision2Options ITvision2Options.UseStartup<TStartup>()
        {
            StartupType = typeof(TStartup);
            return this;
        }

        public ITvision2Options AddHook<T>() where T : class, IEventHook
        {
            if (!_hookTypes.Contains(typeof(T)))
            {
                _hookTypes.Add(typeof(T));
            }
            return this;
        }

    }
}

