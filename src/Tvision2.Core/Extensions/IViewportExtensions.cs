using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Core.Render
{
    public static class IViewportExtensions_Core
    {
        public static bool Intersects(this IViewport vp, IViewport another)
        {
            var otherPos = another.Position;

            var x1 = vp.Position.Left;
            var y1 = vp.Position.Top;
            var x2 = x1 + vp.Columns;
            var y2 = y1 + vp.Rows;

            var x1o = otherPos.Left;
            var y1o = otherPos.Top;
            var x2o = x1o + another.Columns;
            var y2o = y1o + another.Rows;

            return (x2o >= x1 && x1o <= x2) && (y2o >= y1 && y1o <= y2);
        }

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
