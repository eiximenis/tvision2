using System;
using System.Collections.Generic;
using System.Linq;
using Tvision2.Controls.Drawers;
using Tvision2.Controls.Styles;
using Tvision2.Core.Components;
using Tvision2.Core.Components.Behaviors;
using Tvision2.Core.Engine;
using Tvision2.Core.Render;

namespace Tvision2.Controls.Window
{
    public class TvWindow : TvControl<WindowState>
    {
        public TvWindow(ISkin skin, IViewport boxModel, WindowState initialState)
            : base(skin, boxModel.Layer(ViewportLayer.Top, -1), initialState, $"TvWindow_{Guid.NewGuid()}")
        {
            initialState.SetOwnerWindow(this);
            AsComponent().Metadata.ViewportChanged += MyViewportChanged;
        }

        private void MyViewportChanged(object sender, ViewportUpdatedEventArgs e)
        {
            var childs = State.UI.Components.ToList();
            var diff = e.Current.Position - e.Previous.Position;
            foreach (var child in childs)
            {
                child.UpdateViewport(child.Viewport.Translate(diff));
            }
        }

        internal void Close()
        {
            var childs = State.UI.Components.ToList();
            foreach (var child in childs)
            {
                State.UI.Remove(child);
            }
            AsComponent().Metadata.ViewportChanged -= MyViewportChanged;
            State.UI.Engine.UI.Remove(AsComponent());
        }

        protected override IEnumerable<ITvBehavior<WindowState>> GetEventedBehaviors()
        {
            yield return new WindowKeyboardBehavior();
        }

        protected override void AddCustomElements(TvComponent<WindowState> component)
        {
            component.AddDrawer(new BorderDrawer(CurrentStyles));
        }

        protected override void OnDraw(RenderContext<WindowState> context)
        {
            var style = CurrentStyles.GetStyle("");
            for (var row = 1; row < Viewport.Rows - 2; row++)
            {
                context.DrawChars(' ', Viewport.Columns - 2, new TvPoint(1, row), style.ForeColor, style.BackColor);
            }
            context.Fill(style.BackColor);

        }
    }
}
