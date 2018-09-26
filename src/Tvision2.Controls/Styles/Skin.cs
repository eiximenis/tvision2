using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Controls.Styles
{
    class Skin : ISkin
    {
        private readonly IDictionary<string, IStyle> _styles;

        public Skin(IDictionary<string, IStyle> styles)
        {
            _styles = styles ?? throw new ArgumentNullException(nameof(styles));
        }

        public IStyle this[string name]
        {
            get
            {
                var found = _styles.TryGetValue(name, out var sheet);
                return found ? sheet : _styles[""];
            }
        }
    }
}
