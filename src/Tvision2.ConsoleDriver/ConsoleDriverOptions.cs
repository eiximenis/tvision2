using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Colors;

namespace Tvision2.ConsoleDriver
{

    public enum ConsoleItemActionRequested
    {
        None,
        TryEnable,
        TryDisable
    }

    public class ConsoleDriverOptions : IConsoleDriverOptions
    {
        public TvColor DefaultBackColor { get; private set; }
        public ConsoleWindowOptions WindowOptions { get; }

        public ConsoleItemActionRequested MouseActionRequested { get; private set; }

        public ConsoleDriverOptions()
        {
            DefaultBackColor = TvColor.Black;
            WindowOptions = new ConsoleWindowOptions();
            MouseActionRequested = ConsoleItemActionRequested.None;
        }

        IConsoleDriverOptions IConsoleDriverOptions.EnableMouse()
        {
            MouseActionRequested = ConsoleItemActionRequested.TryEnable;
            return this;
        }
        IConsoleDriverOptions IConsoleDriverOptions.DisableMouse()
        {
            MouseActionRequested = ConsoleItemActionRequested.TryDisable;
            return this;
        }

        IConsoleDriverOptions IConsoleDriverOptions.UseBackColor(TvColor backColor)
        {
            DefaultBackColor = backColor;
            return this;
        }

        IConsoleDriverOptions IConsoleDriverOptions.SetupWindow(Action<IConsoleWindowOptions> options)
        {
            options.Invoke(WindowOptions);
            return this;
        }
    }
}
