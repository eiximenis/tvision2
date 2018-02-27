using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Render;

namespace Tvision2.Core.Styles
{
    public class StyleSheet : IBoxModel
    {
        public ClippingMode Clipping { get; }
        private IBaseStyles _parent;

        private readonly List<string> _classes;

        public StyleSheet(ClippingMode clippingMode, IBaseStyles parent = null)
        {
            _backColor = null;
            _foreColor = null;
            Clipping = clippingMode;
            _parent = parent ?? DefaultBaseStyles.Instance;
            _classes = new List<string>();
        }

        public bool IsDirty { get; internal set; }

        private TvPoint _pos;
        public TvPoint Position
        {
            get => _pos;
            set { _pos = value; IsDirty = true; }
        }

        private ConsoleColor? _foreColor;
        public ConsoleColor ForeColor
        {
            get => _foreColor ?? _parent.ForeColor;
            set { _foreColor = value; IsDirty = true; }
        }

        private ConsoleColor? _backColor;
        public ConsoleColor BackColor
        {
            get => _backColor ?? _parent.BackColor;
            set { _backColor = value; IsDirty = true; }
        }

        private int _paddingLeft;
        public int PaddingLeft
        {
            get => _paddingLeft;
            set { _paddingLeft = value; IsDirty = true; }
        }
        private int _paddingRight;
        public int PaddingRight
        {
            get => _paddingRight;
            set { _paddingRight = value; IsDirty = true; }
        }

        private int _columns;
        public int Columns
        {
            get => _columns;
            set { _columns = value; IsDirty = true; }
        }

        private int _rows;
        public int Rows
        {
            get => _rows;
            set { _rows = value; IsDirty = true; }
        }

        public void AddClass(string name)
        {
            if (!_classes.Contains(name))
            {
                _classes.Add(name);
                IsDirty = true;
            }
        }

        public void RemoveClass(string name)
        {
            if (_classes.Contains(name))
            {
                _classes.Remove(name);
                IsDirty = true;
            }
        }

        public bool ContainsClass(string name) => _classes.Contains(name);

        public IEnumerable<string> Classes => _classes;

        private int _zindex;
        public int ZIndex
        {
            get => _zindex;
            set { _zindex = value; IsDirty = true; }
        }
    }
}
