using System;
using Tvision2.ConsoleDriver;
using Tvision2.Engine.Console;

namespace Tvision2.Core.Engine
{
    public static class TuiEngineBuilderExtensions_ConsoleDriver
    {
        public static TuiEngineBuilder UseWin32ConsoleDriver(this TuiEngineBuilder builder, Action<ConsoleDriverOptions> config = null)
        {
            var options = new ConsoleDriverOptions();
            config?.Invoke(options);
            builder.UseConsoleDriver(new WinConsoleDriver(options));
            return builder;
        }

        public static TuiEngineBuilder UseNcursesConsoleDriver(this TuiEngineBuilder builder, Action<ConsoleDriverOptions> config = null)
        {
            var options = new ConsoleDriverOptions();
            config?.Invoke(options);
            builder.UseConsoleDriver(new NcursesConsoleDriver(options));
            return builder;
        }

        public static TuiEngineBuilder UseDotNetConsoleDriver(this TuiEngineBuilder builder, Action<ConsoleDriverOptions> config = null)
        {
            var options = new ConsoleDriverOptions();
            config?.Invoke(options);
            builder.UseConsoleDriver(new NetConsoleDriver(options));
            return builder;
        }

        public static TuiEngineBuilder UsePlatformConsoleDriver(this TuiEngineBuilder builder, Action<ConsoleDriverOptions> config = null)
        {
            var options = new ConsoleDriverOptions();
            config?.Invoke(options);
            var platform = Environment.OSVersion.Platform;
            var useWin = (platform == PlatformID.Win32NT || platform == PlatformID.Win32S || platform == PlatformID.Win32Windows);
            var driver = useWin ? new WinConsoleDriver(options) as IConsoleDriver : new NcursesConsoleDriver(options) as IConsoleDriver;
            builder.UseConsoleDriver(driver);
            return builder;
        }
    }
}
