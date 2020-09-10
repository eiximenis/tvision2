using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Controls.Button
{
    public class ButtonState : IDirtyObject
    {
        public static ButtonState FromText(string value) => new ButtonState() { Text = value };

        public bool IsDirty { get; private set; }

        void IDirtyObject.Validate() => IsDirty = false;

        private bool _isPressed;
        public bool IsPressed
        {
            get => _isPressed;
            set { _isPressed = value; IsDirty = true; }
        }

        private string _text;
        public string Text
        {
            get => _text;
            set { _text = value; IsDirty = true; }
        }
    }
}
