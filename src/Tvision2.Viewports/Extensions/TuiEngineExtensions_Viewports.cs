using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Engine;
using Tvision2.Viewports;

namespace Tvision2.Core.Engine
{
    public static class TuiEngineExtensions_Viewports
    {
        public static IViewportManager ViewportManager(this TuiEngine tui) => tui.GetCustomItem<IViewportManager>();
    }
}
