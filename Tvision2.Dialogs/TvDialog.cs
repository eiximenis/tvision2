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
        public TvDialog(ISkin skin, IViewport viewport, IComponentTree owner) 
            : base(skin, viewport, new DialogState(skin, owner, viewport), $"TvDialog_{Guid.NewGuid()}")
        {

        }


        protected override void AddCustomElements(TvComponent<DialogState> component)
        {
            component.AddDrawer(new BorderDrawer(CurrentStyles));

        }

        protected override void OnDraw(RenderContext<DialogState> context)
        {
        }
    }
}
