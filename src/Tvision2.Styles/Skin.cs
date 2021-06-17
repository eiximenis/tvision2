using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Colors;

namespace Tvision2.Styles
{
    class Skin : ISkin
    {
        internal const string DEFAULT_STYLE_NAME = "";
        private readonly IDictionary<string, IStyle> _styles;
        public IColorManager ColorManager { get; }

        public Skin(IDictionary<string, IStyle> styles, IColorManager colorManager)
        {
            _styles = styles ?? throw new ArgumentNullException(nameof(styles));
        }

        public IStyle DefaultStyle
        {
            get => _styles[DEFAULT_STYLE_NAME];         // TODO: Ensure always is a DEFAULT_STYLE!
        }

        public IStyle this[string name]
        {
            get
            {
                var found = _styles.TryGetValue(name, out var sheet);
                return found ? sheet : DefaultStyle;
            }
        }

        public bool HasStyle(string name) => _styles.ContainsKey(name);
    }
}
