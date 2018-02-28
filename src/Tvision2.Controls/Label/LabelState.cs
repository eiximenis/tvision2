using System;
using Tvision2.Core.Render;
using Tvision2.Core.Styles;

namespace Tvision2.Controls.Label
{
    public class LabelState : TvControlData
    {

        public LabelState(string name = null) : base (new AppliedStyle(ClippingMode.ExpandBoth), name ?? "Label_" + Guid.NewGuid().ToString())
        {
        }

        private string _text;

        public string Text
        {
            get => _text;
            set { _text = value; IsDirty = true; }
        }
    }
}