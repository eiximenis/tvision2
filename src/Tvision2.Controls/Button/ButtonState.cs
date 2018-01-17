using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Controls.Button
{
    public class ButtonState : TvControlState
    {
        public ButtonState()
        {
            Style.BackColor = ConsoleColor.White;
            Style.ForeColor = ConsoleColor.Black;
            Style.PaddingLeft = 1;
            Style.PaddingRight = 1;
        }

        private string _text;

        public string Text
        {
            get => _text;
            set { _text = value; IsDirty = true; }
        }
    }
}
