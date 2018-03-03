using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Controls.Styles
{
    public interface IStyle
    {
        ConsoleColor ForeColor { get; }
        ConsoleColor BackColor { get; }
    }
}
