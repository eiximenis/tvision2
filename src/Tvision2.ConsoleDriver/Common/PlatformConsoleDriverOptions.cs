using System;
using Tvision2.ConsoleDriver.Win32;

namespace Tvision2.ConsoleDriver.Common
{
    class PlatformConsoleDriverOptions : IPlatformConsoleDriverOptions, IPlatformConsoleDriverOptiosnConfigurator
    {
        private readonly LinuxConsoleDriverOptions _linuxOptions;
        private readonly WindowsConsoleDriverOptions _windowsOptions;

        public WindowsConsoleDriverOptions WindowsOptions => _windowsOptions;
        public LinuxConsoleDriverOptions LinuxOptions => _linuxOptions;

        public PlatformConsoleDriverOptions()
        {
            _linuxOptions = new LinuxConsoleDriverOptions();
            _windowsOptions = new WindowsConsoleDriverOptions();
        }
        
        IPlatformConsoleDriverOptiosnConfigurator IPlatformConsoleDriverOptions.Configure(Action<IConsoleDriverOptions> commonOptions = null)
        {
            commonOptions?.Invoke(_linuxOptions);
            commonOptions?.Invoke(_windowsOptions);
            return this;
        }

        IPlatformConsoleDriverOptiosnConfigurator IPlatformConsoleDriverOptiosnConfigurator.OnWindows(Action<IWindowsConsoleDriverOptions> winOptionsSetup)
        {
            winOptionsSetup.Invoke(_windowsOptions);
            return this;
        }

        IPlatformConsoleDriverOptiosnConfigurator IPlatformConsoleDriverOptiosnConfigurator.OnLinux(Action<ILinuxConsoleDriverOptions> linuxOptionsSetup)
        {
            linuxOptionsSetup.Invoke(_linuxOptions);
            return this;
        }
    }
}