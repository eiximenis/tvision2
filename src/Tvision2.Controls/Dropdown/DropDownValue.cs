using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Controls.Dropdown
{
    public struct DropDownValue
    {
        public string Id { get;  }
        public string Text { get; }

        public DropDownValue(string id, string text)
        {
            Id = id;
            Text = text;
        }

        public void Deconstruct(out string id, out string text)
        {
            id = Id;
            text = Text;
        }

    }
}
