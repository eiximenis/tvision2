using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Controls.Drawers;
using Tvision2.Controls.Styles;
using Tvision2.Core.Components;
using Tvision2.Core.Render;

namespace Tvision2.Controls.Window
{
    public class TvWindow : TvControl<WindowState>
    {
        public TvWindow(ISkin skin, IViewport boxModel, WindowState initialState) : base(skin, boxModel, initialState)
        {

        }

        protected override void AddCustomElements(TvComponent<WindowState> component)
        {
            component.AddDrawer(new BorderDrawer(CurrentStyles));
        }

        protected override void OnDraw(RenderContext<WindowState> context)
        {
        }
    }
}
