using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Render;

namespace Tvision2.Controls.Styles
{
    public class AppliedStyle : IBoxModel, IDirtyObject
    {
        public ClippingMode Clipping { get; }
        private readonly List<string> _classes;
        private readonly IStyleSheet _provider;

        public AppliedStyle(IBoxModel boxModel, IStyleSheet provider)
        {
            _provider = provider;
            _foreColor = DefaultStyle.Instance.ForeColor;
            _foreColor = DefaultStyle.Instance.BackColor;
            Clipping = boxModel.Clipping;
            Position = boxModel.Position;
            Columns = boxModel.Columns;
            ZIndex = boxModel.ZIndex;
            Rows = boxModel.Rows;
            _classes = new List<string>();
        }

        public bool IsDirty { get; private set; }
        public void Validate() => IsDirty = false;


        private TvPoint _pos;
        public TvPoint Position
        {
            get => _pos;
            set { _pos = value; IsDirty = true; }
        }

        private ConsoleColor _foreColor;
        public ConsoleColor ForeColor
        {
            get => _foreColor;
            set { _foreColor = value; IsDirty = true; }
        }

        private ConsoleColor _backColor;
        public ConsoleColor BackColor
        {
            get => _backColor;
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
                _provider.UpdateStyle(this);
                IsDirty = true;
            }
        }

        public void RemoveClass(string name)
        {
            if (_classes.Contains(name))
            {
                _classes.Remove(name);
                _provider.UpdateStyle(this);
                IsDirty = true;
            }
        }


        public bool Grow(int cols, int rows)
        {
            var grown = false;
            if (rows > _rows)
            {
                _rows = rows;
                IsDirty = true;
                grown = true;
            }
            if (cols > _columns)
            {
                _columns = cols;
                IsDirty = true;
                grown = true;
            }

            return grown;
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
