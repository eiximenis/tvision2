using System;

namespace Tvision2.Controls.Label
{
    public class LabelState : IDirtyObject
    {


        public static LabelState FromText(string value) => new LabelState() { Text = value };
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