namespace Tvision2.ConsoleDriver.Win32
{
    public class WindowsConsoleDriverOptions : ConsoleDriverOptions, IWindowsConsoleDriverOptions
    {
        public bool UseAnsiSequences { get; private set; }

        public WindowsConsoleDriverOptions()
        {
            UseAnsiSequences = false;
        }
        IWindowsConsoleDriverOptions IWindowsConsoleDriverOptions.EnableAnsiSequences()
        {
            UseAnsiSequences = true;
            return this;
        }

    }
}