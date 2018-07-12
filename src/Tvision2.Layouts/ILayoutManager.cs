using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Layouts
{
    public interface ILayoutManager
    {
        T Get<T>(string name) where T : class, ITvContainer;
        IViewportFactory ViewportFactory { get; }
    }
}
