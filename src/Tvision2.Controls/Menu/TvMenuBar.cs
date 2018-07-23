using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Controls.Styles;
using Tvision2.Core.Render;

namespace Tvision2.Controls.Menu
{
    public class TvMenuBar : TvControl<MenuBarState>
    {
        public TvMenuBar(ISkin skin, IViewport boxModel, MenuBarState data) : base(skin, boxModel, data)
        {
        }

        protected override void OnDraw(RenderContext<MenuBarState> context)
        {
            var style = Metadata.IsFocused ? CurrentStyles.GetStyle("focused") : CurrentStyles.GetStyle("");
            var coordx = 0;
            foreach (var option in State.Options)
            {
                context.DrawStringAt(option, new TvPoint(coordx, 0), style.ForeColor, style.BackColor);
                coordx += option.Length + 1;
            }
        }
    }
}
