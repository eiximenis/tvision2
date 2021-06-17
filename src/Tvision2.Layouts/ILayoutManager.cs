using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Viewports;

namespace Tvision2.Layouts
{
    public interface ILayoutManager
    {
        T? Get<T>(string name) where T : class, ITvContainer;
        IViewportFactory ViewportFactory { get; }
        IDynamicViewportFactory DynamicViewportFactory { get; }
    }
}
