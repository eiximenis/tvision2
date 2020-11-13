using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Colors;

namespace Tvision2.ConsoleDriver
{

    public enum MouseStatus
    {
        Disabled,
        Enabled
    }
    
    public class ConsoleDriverOptions : IConsoleDriverOptions
    {
        public TvColor DefaultBackColor { get; private set; }
        public ConsoleWindowOptions WindowOptions { get; }

        public MouseStatus MouseStatusDesired { get; private set; }

        public ConsoleDriverOptions()
        {
            DefaultBackColor = TvColor.Black;
            WindowOptions = new ConsoleWindowOptions();
            MouseStatusDesired = MouseStatus.Disabled;
        }

        IConsoleDriverOptions IConsoleDriverOptions.EnableMouse()
        {
            MouseStatusDesired = MouseStatus.Enabled;
            return this;
        }
        IConsoleDriverOptions IConsoleDriverOptions.DisableMouse()
        {
            MouseStatusDesired = MouseStatus.Disabled;
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
