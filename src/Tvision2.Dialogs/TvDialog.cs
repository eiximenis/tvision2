using System;
using Tvision2.Controls;
using Tvision2.Controls.Drawers;
using Tvision2.Controls.Styles;
using Tvision2.Core.Components;
using Tvision2.Core.Engine;
using Tvision2.Core.Render;

namespace Tvision2.Dialogs
{
    public class TvDialog : TvControl<DialogState>
    {
        internal TvDialog(ISkin skin, IViewport viewport, IComponentTree owner, string name = null)
            : base(skin, viewport.Layer(ViewportLayer.Top, -1), new DialogState(skin, name ?? $"TvDialog_{Guid.NewGuid()}"))
        {
            Metadata.CanFocus = false;
            State.Init(this, owner);
        }

        protected override void AddCustomElements(TvComponent<DialogState> component)
        {
            component.AddDrawer(new BorderDrawer(CurrentStyle));

        }

        protected override void OnDraw(RenderContext<DialogState> context)
        {
            var pairIdx = CurrentStyle.Standard;
            for (var row = 1; row < Viewport.Rows - 2; row++)
            {
                context.DrawChars(' ', Viewport.Columns - 2, new TvPoint(1, row), pairIdx);
            }
            context.Fill(pairIdx);
        }
    }
}
