﻿using System;
using System.Collections.Generic;
using System.Reflection.PortableExecutable;
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

        public static bool ContainsPoint(this IViewport vp, TvPoint point)
        {
            var x1 = vp.Position.Left;
            var y1 = vp.Position.Top;
            var x2 = x1 + vp.Bounds.Cols;
            var y2 = y1 + vp.Bounds.Rows;

            return (point.Left >= x1 && point.Left <= x2 && point.Top >= y1 && point.Top <= y2);
        }

        public static IViewport Layer(this IViewport viewport, Layer layer, int zIndexDisplacement = 0) 
            => new Viewport(viewport.Position, viewport.Bounds, layer.Move(zIndexDisplacement));

        // InnerViewport
        // Given a "container viewport" creates a child viewport included in this viewport in the given pos and with given maximum bounds.
        // The child viewport is cut to ensure that always fits in the container viewport
        public static IViewport InnerViewport(this IViewport containerViewport, TvPoint pos, TvBounds desiredBounds, TvBounds? maxBounds = null)
        {
            var vppos = pos + containerViewport.Position;
            var displacement = pos;
            
            var availableCols = containerViewport.Bounds.Cols - displacement.Left;
            var availableRows = containerViewport.Bounds.Rows - displacement.Top;

            var vpCols = desiredBounds.Cols < availableCols ? desiredBounds.Cols : availableCols;
            var vpRows = desiredBounds.Rows < availableRows ? desiredBounds.Rows : availableRows;

            var vpBounds = TvBounds.FromRowsAndCols(vpRows, vpCols);

            if (maxBounds.HasValue)
            {
                vpBounds = TvBounds.Min(vpBounds, maxBounds.Value);
            }

            var vp = new Viewport(vppos, vpBounds, containerViewport.ZIndex);
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

        public static IViewport WithBounds(this IViewport viewport, TvBounds newBounds)
        {
            return new Viewport(viewport.Position, newBounds, viewport.ZIndex, viewport.Flow);
        }

    }
}
