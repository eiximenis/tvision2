using System;
using Tvision2.Controls;
using Tvision2.Controls.Drawers;
using Tvision2.Controls.Styles;
using Tvision2.Core.Components;
using Tvision2.Core.Engine;
using Tvision2.Core.Render;
using Tvision2.Controls.Extensions;
using System.Collections.Generic;

namespace Tvision2.Dialogs
{
    public class TvDialog : TvControl<DialogState>
    {
       internal TvDialog(ISkin skin, IViewport viewport, IComponentTree owner, string name = null)
            : base(new TvControlCreationParameters<DialogState>(
                skin, viewport.Layer(Layer.Top, -1),
                new DialogState(skin, name ?? $"TvDialog_{Guid.NewGuid()}")))
        {
            State.Init(this, owner);
        }

        protected override void AddCustomElements(TvComponent<DialogState> component)
        {
            component.AddDrawer(new BorderDrawer(CurrentStyle, Metadata));
        }

        protected override void OnDraw(RenderContext<DialogState> context)
        {
            var pairIdx = CurrentStyle.Standard;
            for (var row = 1; row < context.Viewport.Bounds.Rows - 2; row++)
            {
                context.DrawChars(' ', context.Viewport.Bounds.Cols - 2, TvPoint.FromXY(1, row), pairIdx);
            }
            context.Fill(pairIdx);
        }

        public void Add(ITvControl controlToAdd)
        {
            State.UI.Add(controlToAdd);
        }

        protected override void ConfigureMetadataOptions(TvControlMetadataOptions options)
        {
            options.IsFocused().Never();
        }

        protected override void OnControlMounted(ITuiEngine owner)
        {
            owner.GetControlsTree().CaptureFocus(this.Metadata);
        }

        internal void Close()
        {
            State.Destroy();

            // TODO: Review this -> It is easy to solve
            /*
            State.UI.Engine.UI.Remove(State.MainPanel.AsComponent());
            State.UI.Engine.UI.Remove(AsComponent());

            */
        }
    }
}
