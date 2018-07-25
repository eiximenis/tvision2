using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Core.Render
{
    public static class IViewportExtensions_Core
    {

        public static IViewport Layer(this IViewport viewport, ViewportLayer layer, int zIndexDisplacement = 0) 
            => new Viewport(viewport.Position, viewport.Columns, viewport.Rows, (int)layer + zIndexDisplacement);
        public static IViewport InnerViewport(this IViewport containerViewport, TvPoint pos, int cols, int rows)
        {
            var vppos = pos + containerViewport.Position;
            var displacement = pos;

            var availableCols = containerViewport.Columns - displacement.Left;
            var availableRows = containerViewport.Rows - displacement.Top;
            var vpCols = cols < availableCols ? cols : availableCols;
            var vpRows = rows < availableRows ? rows : availableRows;
            var vp = new Viewport(vppos, vpCols, vpRows, containerViewport.ZIndex);
            return vp;
        }

        public static IViewport InnerViewport(this IViewport containerViewport, IViewport innerViewport, TvPoint displacement)
        {
            if (innerViewport == null)
            {
                return new Viewport(TvPoint.Zero + displacement, containerViewport.Columns, containerViewport.Rows, containerViewport.ZIndex);
            }
            else
            {
                return containerViewport.InnerViewport(innerViewport.Position + displacement, innerViewport.Columns, innerViewport.Rows);
            }
        }
    }
}
