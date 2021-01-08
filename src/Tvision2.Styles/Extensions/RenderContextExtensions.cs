using System;
using Tvision2.Core.Render;

namespace Tvision2.Styles.Extensions
{
    public static class RenderContextExtensions
    {
        public static void DrawChars(this RenderContext context, char value, int count, TvPoint location, StyleEntry style)
        {
            if (style.IsBackgroundFixed)
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
            if (style.IsBackgroundFixed)
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
            if (style.IsBackgroundFixed)
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

        public static StyledRenderContext<T> Styled<T>(this RenderContext<T> ctx, string? style = null)
        {
            var adapter = ctx.GetTag<StyledRenderContextAdatper>();
            if (adapter != null)
            {
                return new StyledRenderContext<T>(ctx, adapter, style ?? "Default");
            }
            throw new InvalidOperationException("Styles are not enabled.");
        }

        public static ISkinManager GetSkinManager(this RenderContext ctx)
        {
            var adapter = ctx.GetTag<StyledRenderContextAdatper>();
            return adapter?.SkinManager ?? throw new InvalidOperationException("Styles are not enabled.");
        }
    }
}
