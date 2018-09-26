using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Controls.Styles;
using Tvision2.Core.Components;
using Tvision2.Core.Components.Behaviors;
using Tvision2.Core.Render;

namespace Tvision2.Controls.Textbox
{
    public class TvTextbox : TvControl<TextboxState>
    {
        public TvTextbox(ISkin skin, IViewport boxModel, TextboxState initialState) : base(skin, boxModel, initialState)
        {
        }


        protected override IEnumerable<ITvBehavior<TextboxState>> GetEventedBehaviors()
        {
            yield return new TextboxBehavior();
        }

        protected override void OnDraw(RenderContext<TextboxState> context)
        {
            var pairIdx = Metadata.IsFocused ? CurrentStyle.Focused : CurrentStyle.Standard;
            context.DrawChars(' ', context.Viewport.Columns, new TvPoint(0, 0), pairIdx);
            if (!string.IsNullOrEmpty(State.Text))
            {
                context.DrawStringAt(State.Text, new TvPoint(0, 0), pairIdx);
            }
        }
    }
}
