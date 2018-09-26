using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Controls.Styles;
using Tvision2.Core.Render;

namespace Tvision2.Controls.Dropdown
{
    public class TvDropdown : TvControl<DropdownState>
    {
        public TvDropdown(ISkin skin, IViewport boxModel, DropdownState initialState) : base(skin, boxModel, initialState)
        {
        }

        protected override void OnDraw(RenderContext<DropdownState> context)
        {
            var pairIdx = Metadata.IsFocused ? CurrentStyle.Focused : CurrentStyle.Standard;
            context.DrawChars(' ', Viewport.Columns, new TvPoint(0, 0), pairIdx);
            int row = 1;
            foreach (var value in State.Values)
            {
                context.DrawStringAt(value, new TvPoint(0, row++), pairIdx);
            }
        }
    }
}
