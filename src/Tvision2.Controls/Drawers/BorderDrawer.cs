using Tvision2.Controls.Styles;
using Tvision2.Core.Components.Draw;
using Tvision2.Core.Render;
using Tvision2.Controls.Extensions;

namespace Tvision2.Controls.Drawers
{
    public class BorderDrawer : ITvDrawer
    {
        private readonly IStyle _style;
        private readonly TvControlMetadata _metadata;

        public BorderDrawer(IStyle style, TvControlMetadata metadata)
        {
            _style = style;
            _metadata = metadata;
        }

        public void Draw(RenderContext context)
        {

            var viewport = context.Viewport;
            var colorIdx = _metadata.IsFocused ? _style.Focused : _style.Standard;
            var columns = viewport.Bounds.Cols;
            var rows = viewport.Bounds.Rows;
            if (rows > 2 && columns > 2)
            {
                context.DrawChars('\u2554', 1, TvPoint.Zero,colorIdx);
                context.DrawChars('\u2557', 1, TvPoint.FromXY(columns - 1, 0), colorIdx);
                context.DrawChars('\u2550', columns - 2, TvPoint.FromXY(1, 0), colorIdx);
                for (var row = 1; row < rows - 1; row++)
                {
                    context.DrawChars('\u2551', 1, TvPoint.FromXY(0, row), colorIdx);
                    context.DrawChars('\u2551', 1, TvPoint.FromXY(columns - 1, row), colorIdx);
                }

                context.DrawChars('\u255a', 1, TvPoint.FromXY(0, rows - 1), colorIdx);
                context.DrawChars('\u255d', 1, TvPoint.FromXY(columns - 1, rows - 1), colorIdx);
                context.DrawChars('\u2550', columns - 2, TvPoint.FromXY(1, rows - 1), colorIdx);
            }
        }
    }
}
