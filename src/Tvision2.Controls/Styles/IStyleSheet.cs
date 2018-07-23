using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Render;

namespace Tvision2.Controls.Styles
{
    public interface IStyleSheet
    {
        void AddClassStyle(string name, IStyle style);
        void RemoveClassStyle(string name);
        IStyle GetStyle(string className);
    }
}
