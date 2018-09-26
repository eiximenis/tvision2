using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Controls.Styles
{
    public interface ISkin
    {
        IStyle this[string name] { get; }
    }
}
