using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Controls.Behavior;
using Tvision2.Controls.Styles;
using Tvision2.Core.Components;
using Tvision2.Core.Render;

namespace Tvision2.Controls.Label
{
    public class TvLabel : TvControlString<LabelState>
    {

        public TvLabel(ISkin skin, IBoxModel boxModel, LabelState state) : base(skin, boxModel, state)
        {
        }

        protected override string GetStringToRender(LabelState state)
        {
            var focused = Style.ContainsClass("focused");
            var value = string.Format("{0}{1}{2}{3}{4}",
                new string(' ', Style.PaddingLeft),
                focused ? ">" : "",
                state.Text.ToString() ?? "",
                focused ? "<" : "",
                new string(' ', Style.PaddingRight));

            return value;
        }


    }
}
