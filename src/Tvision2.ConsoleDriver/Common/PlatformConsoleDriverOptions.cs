using System;
using Tvision2.ConsoleDriver.Win32;

namespace Tvision2.ConsoleDriver.Common
{
    class PlatformConsoleDriverOptions : IPlatformConsoleDriverOptions, IPlatformConsoleDriverOptiosnConfigurator
    {
        public WindowsConsoleDriverOptions WindowsOptions { get; }
        public LinuxConsoleDriverOptions LinuxOptions { get; }

        public PlatformConsoleDriverOptions()
        {
            LinuxOptions = new LinuxConsoleDriverOptions();
            WindowsOptions = new WindowsConsoleDriverOptions();
        }
        
        IPlatformConsoleDriverOptiosnConfigurator IPlatformConsoleDriverOptions.Configure(Action<IConsoleDriverOptions> commonOptions = null)
        {
            commonOptions?.Invoke(LinuxOptions);
            commonOptions?.Invoke(WindowsOptions);
            return this;
        }

        IPlatformConsoleDriverOptiosnConfigurator IPlatformConsoleDriverOptiosnConfigurator.OnWindows(Action<IWindowsConsoleDriverOptions> winOptionsSetup)
        {
            winOptionsSetup.Invoke(WindowsOptions);
            return this;
        }

        IPlatformConsoleDriverOptiosnConfigurator IPlatformConsoleDriverOptiosnConfigurator.OnLinux(Action<ILinuxConsoleDriverOptions> linuxOptionsSetup)
        {
            linuxOptionsSetup.Invoke(LinuxOptions);
            return this;
        }
    }
}