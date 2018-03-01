using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Core.Styles
{
    public interface IStyleSheet
    {
        void AddClassStyle(string name, IStyle style);
        void RemoveClassStyle(string name);
        ConsoleColor GetForeColor(IEnumerable<string> classes);
        ConsoleColor GetBackColor(IEnumerable<string> classes);


    }
}
