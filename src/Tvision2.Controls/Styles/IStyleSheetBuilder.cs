using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Controls.Styles
{
    public interface IStyleSheetBuilder
    {
        IStyleSheetBuilder AddClass(string name, IStyle style);
        IStyleSheetBuilder AddClass(string name, Action<IStyleBuilder> action);
        IStyleSheet Build();
    }
}
