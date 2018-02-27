using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.ConsoleDriver;
using Tvision2.Core.Hooks;

namespace Tvision2.Core.Engine
{
    public class TuiEngineBuilder
    {
        private readonly Dictionary<Type, object> _customItems;
        private ConsoleDriverType _consoleDriverType;
        private readonly List<IEventHook> _hooks;

        public TuiEngineBuilder()
        {
            _customItems = new Dictionary<Type, object>();
            _consoleDriverType = ConsoleDriverType.PlatformDriver;
            _hooks = new List<IEventHook>();
        }

        public TuiEngine Build()
        {
            var consoleDriver = BuildConsoleDriver();
            return new TuiEngine(consoleDriver, _customItems, _hooks);
        }

        public TuiEngineBuilder AddEventHook(IEventHook hook)
        {
            _hooks.Add(hook);
            return this;
        }

        public TuiEngineBuilder UseConsoleDriver(ConsoleDriverType driverToUse)
        {
            _consoleDriverType = driverToUse;
            return this;
        }

        private IConsoleDriver BuildConsoleDriver()
        {
            switch (_consoleDriverType)
            {
                case ConsoleDriverType.NCursesDriver:
                    return new NcursesConsoleDriver();
                case ConsoleDriverType.NetDriver:
                    return new NetConsoleDriver();
                case ConsoleDriverType.WindowsDriver:
                    return new WinConsoleDriver();
                default:
                    var platform = Environment.OSVersion.Platform;
                    var useWin = (platform == PlatformID.Win32NT || platform == PlatformID.Win32S || platform == PlatformID.Win32Windows);
                    return useWin ? new WinConsoleDriver() as IConsoleDriver : new NcursesConsoleDriver() as IConsoleDriver;
            }
        }

        public void SetCustomItem<T>(T item)
        {
            _customItems.Add(typeof(T),item);
        }
    }
}
