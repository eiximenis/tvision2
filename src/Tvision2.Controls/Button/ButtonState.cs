using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Render;
using Tvision2.Core.Styles;

namespace Tvision2.Controls.Button
{
    public class ButtonState : TvControlData
    {
        public ButtonState(string name = null) : base(new StyleSheet(ClippingMode.Clip), name ?? "Button" + Guid.NewGuid().ToString())
        {
            Style.BackColor = ConsoleColor.White;
            Style.ForeColor = ConsoleColor.Black;
            Style.PaddingLeft = 1;
            Style.PaddingRight = 1;
            Style.Columns = 4;
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
