using System.Collections.Concurrent;
using System.Reflection;
using Tvision2.Core.Components.Draw;
using Tvision2.Core.Render;
using Tvision2.Styles;
using Tvision2.Styles.Extensions;

namespace Tvision2.Styles.Drawers
{
    public class BorderDrawer : ITvDrawer
    {
        private readonly IStyle _style;
        private readonly BorderValue _border;
        private readonly char[] _borderSet;

        public BorderDrawer(IStyle style, BorderValue border)
        {
            _style = style;
            _border = border;
            _borderSet = BorderSets.GetBorderSet(_border);
        }

        public DrawResult Draw(RenderContext context)
        {

            var viewport = context.Viewport;
            var entry = _style.Standard;
            var columns = viewport.Bounds.Cols;
            var rows = viewport.Bounds.Rows;

            var minrows = _border.HasHorizontalBorder ? 2 : 0;
            var mincols = _border.HasVerticalBorder ? 2 : 0;


            if (rows > minrows && columns > mincols)
            {
                var displacement = TvPoint.FromXY(_border.HasVerticalBorder ? 1 : 0, _border.HasHorizontalBorder ? 1 : 0);
                var adjustement = TvBounds.FromRowsAndCols(_border.HasHorizontalBorder ? 2 : 0, _border.HasVerticalBorder ? 2 : 0);


                if (_border.HasHorizontalBorder)
                {
                    context.DrawChars(_borderSet[BorderSets.Entries.TOPLEFT], 1, TvPoint.Zero, entry);
                    context.DrawChars(_borderSet[BorderSets.Entries.TOPRIGHT], 1, TvPoint.FromXY(columns - 1, 0), entry);
                    context.DrawChars(_borderSet[BorderSets.Entries.HORIZONTAL], columns - 2, TvPoint.FromXY(1, 0), entry);
                    context.DrawChars(_borderSet[BorderSets.Entries.BOTTOMLEFT], 1, TvPoint.FromXY(0, rows - 1), entry);
                    context.DrawChars(_borderSet[BorderSets.Entries.BOTTOMRIGHT], 1, TvPoint.FromXY(columns - 1, rows - 1), entry);
                    context.DrawChars(_borderSet[BorderSets.Entries.HORIZONTAL], columns - 2, TvPoint.FromXY(1, rows - 1), entry);
                }
                if (_border.HasVerticalBorder)
                {
                    var startRow = _border.HasHorizontalBorder ? 1 : 0;
                    var endrow = _border.HasHorizontalBorder ? rows - 1 : rows - 2;
                    for (var row = startRow; row < endrow; row++)
                    {
                        context.DrawChars(_borderSet[BorderSets.Entries.VERTICAL], 1, TvPoint.FromXY(0, row), entry);
                        context.DrawChars(_borderSet[BorderSets.Entries.VERTICAL], 1, TvPoint.FromXY(columns - 1, row), entry);
                    }
                }

                return new DrawResult(displacement, adjustement);
            }
            return DrawResult.Done;
        }
    }
}
