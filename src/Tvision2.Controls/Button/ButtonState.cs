using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Controls.Button
{
    public class ButtonState : TvControlData
    {
        public ButtonState()
        {
            Style.BackColor = ConsoleColor.White;
            Style.ForeColor = ConsoleColor.Black;
            Style.PaddingLeft = 1;
            Style.PaddingRight = 1;
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
