using Tvision2.Controls.Styles;
using Tvision2.Core.Components.Draw;
using Tvision2.Core.Render;

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
            
            if (viewport.Rows > 2 && viewport.Columns > 2)
            {
                context.DrawChars('\u2554', 1, TvPoint.Zero,colorIdx);
                context.DrawChars('\u2557', 1, new TvPoint(viewport.Columns - 1, 0), colorIdx);
                context.DrawChars('\u2550', viewport.Columns - 2, new TvPoint(1, 0), colorIdx);
                for (var row = 1; row < viewport.Rows - 1; row++)
                {
                    context.DrawChars('\u2551', 1, new TvPoint(0, row), colorIdx);
                    context.DrawChars('\u2551', 1, new TvPoint(viewport.Columns - 1, row), colorIdx);
                }

                context.DrawChars('\u255a', 1, new TvPoint(0, viewport.Rows - 1), colorIdx);
                context.DrawChars('\u255d', 1, new TvPoint(viewport.Columns - 1, viewport.Rows - 1), colorIdx);
                context.DrawChars('\u2550', viewport.Columns - 2, new TvPoint(1, viewport.Rows - 1), colorIdx);
            }
        }
    }
}
