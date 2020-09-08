using Tvision2.Core.Components.Draw;
using Tvision2.Core.Render;
using Tvision2.Styles;
using Tvision2.Styles.Extensions;

namespace Tvision2.Styles.Drawers
{
    public class BorderDrawer : ITvDrawer
    {
        private readonly IStyle _style;

        public BorderDrawer(IStyle style)
        {
            _style = style;
        }

        public DrawResult Draw(RenderContext context)
        {

            var viewport = context.Viewport;
            var entry = _style.Standard;
            var columns = viewport.Bounds.Cols;
            var rows = viewport.Bounds.Rows;
            if (rows > 2 && columns > 2)
            {
                context.DrawChars('\u2554', 1, TvPoint.Zero, entry);
                context.DrawChars('\u2557', 1, TvPoint.FromXY(columns - 1, 0), entry);
                context.DrawChars('\u2550', columns - 2, TvPoint.FromXY(1, 0), entry);
                for (var row = 1; row < rows - 1; row++)
                {
                    context.DrawChars('\u2551', 1, TvPoint.FromXY(0, row), entry);
                    context.DrawChars('\u2551', 1, TvPoint.FromXY(columns - 1, row), entry);
                }

                context.DrawChars('\u255a', 1, TvPoint.FromXY(0, rows - 1), entry);
                context.DrawChars('\u255d', 1, TvPoint.FromXY(columns - 1, rows - 1), entry);
                context.DrawChars('\u2550', columns - 2, TvPoint.FromXY(1, rows - 1), entry);
                return new DrawResult(TvPoint.FromXY(1, 1), TvBounds.FromRowsAndCols(2, 2));
            }
            return DrawResult.Done;
        }
    }
}
