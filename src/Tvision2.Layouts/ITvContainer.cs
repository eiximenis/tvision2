using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core;
using Tvision2.Core.Components;
using Tvision2.Core.Engine;

namespace Tvision2.Layouts
{

    public interface ITvContainer  
    {
        TvComponent AsComponent();
        string Name { get; }
    }
}
