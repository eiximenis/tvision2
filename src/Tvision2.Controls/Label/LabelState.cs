using System;
using Tvision2.Controls.Styles;
using Tvision2.Core.Render;

namespace Tvision2.Controls.Label
{
    public class LabelState : IDirtyObject
    {

        public bool IsDirty { get; private set; }

        void IDirtyObject.Validate() => IsDirty = false;

        private string _text;
        public string Text
        {
            get => _text;
            set { _text = value; IsDirty = true; }
        }
    }
}