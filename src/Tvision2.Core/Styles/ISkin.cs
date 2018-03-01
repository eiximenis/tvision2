using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Core.Styles
{
    public interface ISkin
    {
        IStyleSheet this[string name] { get; }
    }
}
