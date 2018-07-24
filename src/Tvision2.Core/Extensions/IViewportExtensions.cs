using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Core.Render
{
    public static class IViewportExtensions_Core
    {
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

        public static IViewport InnerViewport(this IViewport containerViewport, IViewport innerViewport)
        {
            if (innerViewport == null)
            {
                return new Viewport(new TvPoint(0, 0), containerViewport.Columns, containerViewport.Rows, containerViewport.ZIndex);
            }
            else
            {
                return containerViewport.InnerViewport(innerViewport.Position, innerViewport.Columns, innerViewport.Rows);
            }
        }
    }
}
