using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.ConsoleDriver;

namespace Tvision2.Core.Engine
{
    public class TuiEngineBuilder
    {
        private readonly Dictionary<string, object> _customItems;
        private ConsoleDriverType _consoleDriverType;

        public TuiEngineBuilder()
        {
            _customItems = new Dictionary<string, object>();
            _consoleDriverType = ConsoleDriverType.PlatformDriver;
        }

        public TuiEngine Build()
        {
            var consoleDriver = BuildConsoleDriver();
            return new TuiEngine(consoleDriver, _customItems);
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

        public void SetCustomItem(string key, object item)
        {
            _customItems.Add(key, item);
        }
    }
}
