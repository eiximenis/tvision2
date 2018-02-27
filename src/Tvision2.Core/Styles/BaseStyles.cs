using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Core.Styles
{
    class BaseStyles : IBaseStyles
    {
        private readonly IBaseStyles _parent;
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
        public BaseStyles(IBaseStyles parent) => _parent = parent ?? DefaultBaseStyles.Instance;

    }
}
