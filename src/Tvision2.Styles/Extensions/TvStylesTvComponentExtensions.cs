using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Components;
using Tvision2.Styles.Drawers;

namespace Tvision2.Styles.Extensions
{
    public static class TvStylesTvComponentExtensions
    {

        public static TvComponent<T> WithDoubleBorder<T>(this TvComponent<T> component, string styleToUse = null)
        {
            component.Metadata.OnTreeUpdatedByMount.AddOnce(mctx =>
            {
                mctx.Component.InsertDrawerAt(new BorderDrawer(mctx.Component.GetStyle(styleToUse)), 0);
            });
            return component;
        }

        public static IStyle GetStyle (this TvComponent component, string defaultStyle = null)
        {
            var adapter = component.Metadata.GetTag<StyledRenderContextAdatper>() ?? throw new InvalidOperationException("Styles are not enabled");
            var style = adapter.SkinManager.CurrentSkin.GetStyleForComponent(component, defaultStyle);
            return style;
        }

        public static ISkinManager GetSkinManager(this TvComponent component)
        {
            var adapter = component.Metadata.GetTag<StyledRenderContextAdatper>() ?? throw new InvalidOperationException("Styles are not enabled");
            return adapter.SkinManager;
        }
    }
}
