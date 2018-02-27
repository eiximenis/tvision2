using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Render;
using Tvision2.Core.Styles;

namespace Tvision2.Controls.Button
{
    public class ButtonState : TvControlData
    {
        public ButtonState(string name = null) : base(new StyleSheet(ClippingMode.ExpandHorizontal), name ?? "Button" + Guid.NewGuid().ToString())
        {
            Style.BackColor = ConsoleColor.White;
            Style.ForeColor = ConsoleColor.Black;
            Style.Columns = 12;
        }

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
