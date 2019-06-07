namespace Tvision2.ConsoleDriver.Win32
{
    public interface IWindowsConsoleDriverOptions : IConsoleDriverOptions
    {
        IWindowsConsoleDriverOptions EnableAnsiSequences();
    }
}