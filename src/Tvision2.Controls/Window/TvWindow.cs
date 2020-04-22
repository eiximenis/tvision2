using System;
using System.Collections.Generic;
using System.Linq;
using Tvision2.Controls.Drawers;
using Tvision2.Controls.Styles;
using Tvision2.Core.Components;
using Tvision2.Core.Components.Behaviors;
using Tvision2.Core.Engine;
using Tvision2.Core.Render;
using Tvision2.Controls.Extensions;
using System.Reflection;

namespace Tvision2.Controls.Window
{
    public class TvWindow : TvControl<WindowState>
    {
        public TvWindow(ITvControlCreationParametersBuilder<WindowState> parameters) : base(parameters.Build()) { }
        public TvWindow(TvControlCreationParameters<WindowState> parameters) : base(parameters)
        {
        }

        private ITuiEngine _engine;

        protected override void OnControlMounted(ITuiEngine engine)
        {
            State.SetOwnerWindow(this);
            AsComponent().Metadata.ViewportChanged += MyViewportChanged;
            _engine = engine;
        }

        protected override void OnControlUnmounted(ITuiEngine engine)
        {
            _engine = null;
        }

        private void MyViewportChanged(object sender, ViewportUpdatedEventArgs e)
        {
            var childs = State.UI.ToList();
            var diff = e.Current.Position - e.Previous.Position;
            foreach (var child in childs)
            {
                child.UpdateViewport(child.Viewport.Translate(diff));
            }
        }

        internal void Close()
        {
            var childs = State.UI.ToList();
            foreach (var child in childs)
            {
                State.UI.Remove(child);
            }
            AsComponent().Metadata.ViewportChanged -= MyViewportChanged;
            _engine.UI.Remove(AsComponent());
        }

        protected override IEnumerable<ITvBehavior<WindowState>> GetEventedBehaviors()
        {
            yield return new WindowKeyboardBehavior();
        }

        protected override void AddCustomElements(TvComponent<WindowState> component)
        {
            component.AddDrawer(new BorderDrawer(CurrentStyle, Metadata));
        }

        protected override void OnDraw(RenderContext<WindowState> context)
        {
            var pairIdx = CurrentStyle.Standard;
            for (var row = 1; row < context.Viewport.Bounds.Rows - 2; row++)
            {
                context.DrawChars(' ', context.Viewport.Bounds.Cols - 2, TvPoint.FromXY(1, row), pairIdx);
            }
            context.Fill(pairIdx);

        }
    }
}
