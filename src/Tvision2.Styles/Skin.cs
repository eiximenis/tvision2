using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Colors;

namespace Tvision2.Styles
{
    class Skin : ISkin
    {
        private readonly IDictionary<string, IStyle> _styles;
        public IColorManager ColorManager { get; }

        public Skin(IDictionary<string, IStyle> styles, IColorManager colorManager)
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
