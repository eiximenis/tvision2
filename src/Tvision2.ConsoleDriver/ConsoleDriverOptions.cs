using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.ConsoleDriver
{
    public class ConsoleDriverOptions : IConsoleDriverOptions
    {
        public ConsoleColor DefaultBackColor { get; private set; }
        public ConsoleColor DefaultForeColor { get; private set; }
        public ConsoleWindowOptions WindowOptions { get; }

        public ConsoleDriverOptions()
        {
            DefaultBackColor = ConsoleColor.Black;
            DefaultBackColor = ConsoleColor.White;
            WindowOptions = new ConsoleWindowOptions();
        }

        IConsoleDriverOptions IConsoleDriverOptions.UseBackColor(ConsoleColor backColor)
        {
            DefaultBackColor = backColor;
            return this;
        }

        IConsoleDriverOptions IConsoleDriverOptions.UseForegroundColor(ConsoleColor foreColor)
        {
            DefaultForeColor = foreColor;
            return this;
        }

        IConsoleDriverOptions IConsoleDriverOptions.SetupWindow(Action<IConsoleWindowOptions> options)
        {
            options.Invoke(WindowOptions);
            return this;
        }
    }
}
