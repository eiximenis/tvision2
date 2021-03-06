﻿using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Components;
using Tvision2.Styles.Drawers;

namespace Tvision2.Styles.Extensions
{
    public static class TvStylesTvComponentExtensions
    {


        public static TvComponent<T> WithBorder<T>(this TvComponent<T> component, BorderValue value, string? styleToUse = null, ISkin? skinToUse = null )
        {
            if (value.HasHorizontalBorder || value.HasVerticalBorder)
            {
                component.Metadata.OnTreeUpdatedByMount.AddOnce(mctx =>
                {
                    mctx.Component.InsertDrawerAt(new BorderDrawer(mctx.Component.GetStyle(styleToUse, skinToUse), value), 0);
                });
            }
            return component;

        }

        public static TvComponent<T> WithDoubleBorder<T>(this TvComponent<T> component, string? styleToUse = null, ISkin? skinToUse = null) => WithBorder(component, BorderValue.Double(), styleToUse, skinToUse);
        public static TvComponent<T> WithSingleBorder<T>(this TvComponent<T> component, string? styleToUse = null, ISkin? skinToUse = null) => WithBorder(component, BorderValue.Single(), styleToUse, skinToUse);

        public static IStyle GetStyle (this TvComponent component, string? defaultStyle = null, ISkin? skinToUse = null)
        {
            var adapter = component.Metadata.GetTag<StyledRenderContextAdatper>() ?? throw new InvalidOperationException("Styles are not enabled");

            var skin = skinToUse ?? adapter.SkinManager.GetDefaultSkin();
            var style = skin.GetStyleForComponent(component, defaultStyle);
            return style;
        }

        public static ISkinManager GetSkinManager(this TvComponent component)
        {
            var adapter = component.Metadata.GetTag<StyledRenderContextAdatper>() ?? throw new InvalidOperationException("Styles are not enabled");
            return adapter.SkinManager;
        }
    }
}
