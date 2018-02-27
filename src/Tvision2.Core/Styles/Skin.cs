using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Core.Styles
{
    class Skin : ISkin
    {
        private readonly Dictionary<string, IBaseStyles> _styles;
        private IBaseStyles _defaultStyle;

        public Skin()
        {
            _styles = new Dictionary<string, IBaseStyles>();
            _defaultStyle = DefaultBaseStyles.Instance;
        }

        internal void AddNamedStyle(string name, IBaseStyles style) => _styles.Add(name, style);

        internal void SetDefaultStyle(IBaseStyles styles) => _defaultStyle = styles ?? DefaultBaseStyles.Instance;

        public IBaseStyles this[string name]
        {
            get
            {
                var found = _styles.TryGetValue(name, out var style);
                return found ? style : _defaultStyle;
            }
        }
    }
}
