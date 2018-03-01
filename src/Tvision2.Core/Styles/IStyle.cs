using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Core.Styles
{
    public interface IStyle
    {
        ConsoleColor ForeColor { get; }
        ConsoleColor BackColor { get; }
    }
}
