using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.ConsoleDriver
{
    public class ConsoleDriverOptions
    {
        public ConsoleColor DefaultBackColor { get; set; }
        public ConsoleColor DefaultForeColor { get; set; }

        public ConsoleDriverOptions()
        {
            DefaultBackColor = ConsoleColor.Black;
            DefaultBackColor = ConsoleColor.White;
        }


    }
}
