using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Controls.Styles
{
    class Style : IStyle
    {
        private readonly IStyle _parent;
        private ConsoleColor? _foreColor;
        private ConsoleColor? _backColor;

        public ConsoleColor ForeColor
        {
            get => _foreColor.HasValue ? _foreColor.Value : _parent.ForeColor;
            internal set
            {
                _foreColor = value;
            }
        }
        public ConsoleColor BackColor
        {
            get => _backColor.HasValue ? _backColor.Value : _parent.BackColor;
            internal set
            {
                _backColor = value;
            }
        }
        public Style(IStyle parent) => _parent = parent ?? DefaultStyle.Instance;

    }
}
