using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Controls;

namespace Tvision2.Core.Components
{
    public static class ComponentMountingContextExtensions_TvControls
    {
        public static TvControlMetadata ControlMetadata(this ComponentMoutingContext ctx)
        {
            return ctx.Node.GetTag<TvControlMetadata>();
        }
    }
}
