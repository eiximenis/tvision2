using System;

namespace Tvision2.ConsoleDriver
{
    public interface IConsoleDriverOptions
    {
        IConsoleDriverOptions UseBackColor(ConsoleColor backColor);
        IConsoleDriverOptions UseForegroundColor(ConsoleColor foreColor);

        IConsoleDriverOptions SetupWindow(Action<IConsoleWindowOptions> options);
    }
}