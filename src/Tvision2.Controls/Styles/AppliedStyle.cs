using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Render;

namespace Tvision2.Controls.Styles
{
    public class AppliedStyle : IDirtyObject
    {
        public ClippingMode Clipping { get; }
        private readonly List<string> _classes;
        private readonly IStyleSheet _provider;
        public AppliedStyle(IStyleSheet provider)
        {
            _provider = provider;
            _foreColor = DefaultStyle.Instance.ForeColor;
            _backColor = DefaultStyle.Instance.BackColor;
            _classes = new List<string>();
        }

        public bool IsDirty { get; private set; }
        public void Validate() => IsDirty = false;

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
        public bool ContainsClass(string name) => _classes.Contains(name);
        public IEnumerable<string> Classes => _classes;
    }
}
