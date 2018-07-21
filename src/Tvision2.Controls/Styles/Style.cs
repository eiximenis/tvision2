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
        private ConsoleColor? _hiBackColor;
        private ConsoleColor? _hiForeColor;

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

        public ConsoleColor HiliteForeColor
        {
            get => _hiForeColor.HasValue ? _hiForeColor.Value : _parent.HiliteForeColor;
            internal set
            {
                _hiForeColor = value;
            }
        }


        public ConsoleColor HiliteBackColor
        {
            get => _hiBackColor.HasValue ? _hiBackColor.Value : _parent.HiliteBackColor;
            internal set
            {
                _hiBackColor = value;
            }
        }

        public Style(IStyle parent) => _parent = parent ?? DefaultStyle.Instance;

    }
}
