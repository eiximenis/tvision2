using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Controls.Styles;
using Tvision2.Core.Render;

namespace Tvision2.Controls.Extensions
{
    public static class RenderContextExtensions
    {
        public static void DrawChars(this RenderContext context, char value, int count, TvPoint location, StyleEntry style)
        {
            if (style.Background.IsFixedBackgroundColor)
            {
                context.DrawChars(value, count, location, style.ToCharacterAttribute(location, context.Viewport.Bounds));
            }
            else
            {
                for (var idx=0; idx < count; idx++)
                {
                    var point = TvPoint.FromXY(location.Left + idx, location.Top);
                    context.DrawChars(value, 1, point, style.ToCharacterAttribute(point, context.Viewport.Bounds));
                }
            }
        }

        public static void DrawStringAt(this RenderContext context, string value, TvPoint location, StyleEntry style)
        {
            if (style.Background.IsFixedBackgroundColor)
            {
                context.DrawStringAt(value, location, style.ToCharacterAttribute(location, context.Viewport.Bounds));
            }
            else
            {
                for (var idx = 0; idx < value.Length; idx++)
                {
                    var point = TvPoint.FromXY(location.Left + idx, location.Top);
                    context.DrawChars(value[idx], 1, point, style.ToCharacterAttribute(point, context.Viewport.Bounds));
                }
            }
        }

        public static void Fill(this RenderContext context, StyleEntry style)
        {
            if (style.Background.IsFixedBackgroundColor)
            {
                context.Fill(style.ToCharacterAttribute(TvPoint.Zero, context.Viewport.Bounds));
            }
            else
            {
                var bounds = context.Viewport.Bounds;
                for (var row = 0; row < bounds.Rows; row++)
                {
                    for (var col = 0; col < bounds.Cols; col++)
                    {
                        var point = TvPoint.FromXY(col, row);
                        context.DrawChars(' ', 1, point, style.ToCharacterAttribute(point, context.Viewport.Bounds));
                    }
                }
            }
        }
    }
}
