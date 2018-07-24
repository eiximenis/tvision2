using System;
using System.Collections.Generic;
using System.Linq;
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
            initialState.SetOwnerWindow(this);
        }


        public void Close()
        {
            var childs = State.Components.ToList();
            foreach (var child in childs)
            {
                State.Remove(child);
            }
            State.Engine.UI.Remove(this.AsComponent());
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
