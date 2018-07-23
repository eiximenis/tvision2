using System;

namespace Tvision2.Controls.Styles
{
    public struct ColorScheme
    {
        public ConsoleColor BackColor { get; }
        public ConsoleColor ForeColor { get; }

        public ColorScheme(ConsoleColor fore, ConsoleColor back)
        {
            ForeColor = fore;
            BackColor = back;
        }
    }
}
