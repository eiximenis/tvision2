using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Tvision2.Core.Render;

namespace Tvision2.Controls.Extensions
{
    public static class TvControlsRenderContextExtensions
    {
        public static TIOptions GetControlOptions<TIOptions> (this RenderContext rc)
        {
            return rc.GetTag<TIOptions>();
        }
    }
}
