using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Tvision2.Core.Components;

namespace Tvision2.Styles.Extensions
{
    public static class SkinExtensions
    {
        public static IStyle GetStyleForComponent(this ISkin skin, TvComponent component, string defaultStyle = null)
        {
            if (string.IsNullOrEmpty(defaultStyle))
            {
                return skin[component.Name];
            }
            else
            {
                return skin.HasStyle(component.Name) ? skin[component.Name] : skin[defaultStyle];
            }
        }
    }
}
