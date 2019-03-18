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
            var x2 = x1 + vp.Bounds.Cols;
            var y2 = y1 + vp.Bounds.Rows;

            var x1o = otherPos.Left;
            var y1o = otherPos.Top;
            var x2o = x1o + another.Bounds.Cols;
            var y2o = y1o + another.Bounds.Rows;

            return (x2o >= x1 && x1o <= x2) && (y2o >= y1 && y1o <= y2);
        }

        public static IViewport Layer(this IViewport viewport, ViewportLayer layer, int zIndexDisplacement = 0) 
            => new Viewport(viewport.Position, viewport.Bounds, (int)layer + zIndexDisplacement);
        public static IViewport InnerViewport(this IViewport containerViewport, TvPoint pos, TvBounds bounds)
        {
            var vppos = pos + containerViewport.Position;
            var displacement = pos;

            var availableCols = containerViewport.Bounds.Cols - displacement.Left;
            var availableRows = containerViewport.Bounds.Rows - displacement.Top;
            var vpCols = bounds.Cols < availableCols ? bounds.Cols : availableCols;
            var vpRows = bounds.Rows < availableRows ? bounds.Rows : availableRows;
            var vp = new Viewport(vppos, new TvBounds(vpRows, vpCols), containerViewport.ZIndex);
            return vp;
        }

        public static IViewport InnerViewport(this IViewport containerViewport, IViewport innerViewport, TvPoint displacement)
        {
            if (innerViewport == null)
            {
                return new Viewport(TvPoint.Zero + displacement, containerViewport.Bounds, containerViewport.ZIndex);
            }
            else
            {
                return containerViewport.InnerViewport(innerViewport.Position + displacement, innerViewport.Bounds);
            }
        }
    }
}
