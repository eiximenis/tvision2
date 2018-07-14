using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tvision2.Controls.Drawers;
using Tvision2.Controls.Styles;
using Tvision2.Core.Components;
using Tvision2.Core.Render;

namespace Tvision2.Controls.List
{
    public class TvList :  TvControl<ListState>
    {
        public TvList(ISkin skin, IViewport boxModel, ListState data) : base(skin, boxModel, data)
        {
        }

        protected override void AddCustomElements(TvComponent<ListState> component)
        {
            component.AddDrawer(new BorderDrawer(Style));
        }

        protected override void OnDraw(RenderContext<ListState> context)
        {
            var viewport = context.Viewport;
            var items = context.State.Values.Take(viewport.Rows - 2);
            var top = 1;
            foreach (var item in items)
            {
                context.DrawStringAt(item, new TvPoint(1, top), Style.BackColor, Style.ForeColor);
                top++;
            }
        }
    }
}
