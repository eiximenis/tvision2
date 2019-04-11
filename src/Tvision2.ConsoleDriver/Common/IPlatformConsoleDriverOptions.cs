using System;
using Tvision2.ConsoleDriver.Win32;

namespace Tvision2.ConsoleDriver.Common
{
    public interface IPlatformConsoleDriverOptions
    {
        IPlatformConsoleDriverOptiosnConfigurator Configure(Action<IConsoleDriverOptions> commonOptions = null);
    }
    
    public interface IPlatformConsoleDriverOptiosnConfigurator 
    {
        IPlatformConsoleDriverOptiosnConfigurator OnWindows(Action<IWindowsConsoleDriverOptions> winOptionsSetup);
        IPlatformConsoleDriverOptiosnConfigurator OnLinux(Action<ILinuxConsoleDriverOptions> linuxOptionsSetup);
    
    }
}