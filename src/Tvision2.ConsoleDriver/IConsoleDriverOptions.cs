using System;
using Tvision2.Core.Colors;

namespace Tvision2.ConsoleDriver
{
    public interface IConsoleDriverOptions
    {
        IConsoleDriverOptions UseBackColor(TvColor bakColor);

        IConsoleDriverOptions SetupWindow(Action<IConsoleWindowOptions> options);
        IConsoleDriverOptions EnableMouse();
        IConsoleDriverOptions DisableMouse();
    }
}