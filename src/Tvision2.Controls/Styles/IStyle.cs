using System;

namespace Tvision2.Controls.Styles
{
    public interface IStyle
    {
        ConsoleColor ForeColor { get; }
        ConsoleColor BackColor { get; }
        ConsoleColor HiliteForeColor { get; }
        ConsoleColor HiliteBackColor { get; }
    }
}
