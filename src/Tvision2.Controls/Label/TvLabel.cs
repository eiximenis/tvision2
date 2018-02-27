using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Controls.Behavior;
using Tvision2.Controls.Draw;
using Tvision2.Core.Components;
using Tvision2.Core.Components.Render;

namespace Tvision2.Controls.Label
{
    public class TvLabel : TvControlString<LabelState>
    {

        public TvLabel(LabelState state) : base(state, state)
        {
        }

        protected override string GetStringToRender(LabelState state)
        {
            var focused = state.Style.ContainsClass("focused");
            var style = state.Style;

            var value = string.Format("{0}{1}{2}{3}{4}",
                new string(' ', style.PaddingLeft),
                focused ? ">" : "",
                state.Text.ToString() ?? "",
                focused ? "<" : "",
                new string(' ', style.PaddingRight));

            return value;
        }


    }
}
