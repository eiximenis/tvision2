using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Controls.Styles;

namespace Tvision2.Core.Engine
{
    public static class TuiEngineExtensions
    {
        public static ISkinManager SkinManager(this TuiEngine engine) => engine.GetCustomItem<ISkinManager>();
    }
}
