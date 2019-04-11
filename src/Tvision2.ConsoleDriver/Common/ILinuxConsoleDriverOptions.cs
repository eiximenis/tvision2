using System;

namespace Tvision2.ConsoleDriver.Common
{
    public interface ILinuxConsoleDriverOptions : IConsoleDriverOptions
    {
        ILinuxConsoleDriverOptions UseDirectAccess(Action<IDirectAccessOptions> directAccessOptions = null);
    }
}