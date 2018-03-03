using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Controls.Styles
{
    class Skin : ISkin
    {
        private readonly IDictionary<string, IStyleSheet> _styleSheets;
        private IStyleSheet _defaultSheet;

        public Skin(IDictionary<string, IStyleSheet> styleSheets)
        {
            _styleSheets = styleSheets ?? throw new ArgumentNullException(nameof(styleSheets));
            _defaultSheet = StyleSheet.Default;
        }

        public void SetDefaultStyleSheet(IStyleSheet sheet) => _defaultSheet = sheet;

        public IStyleSheet DefaultStyles => _defaultSheet;

        public IStyleSheet this[string name]
        {
            get
            {
                var found = _styleSheets.TryGetValue(name, out var sheet);
                return found ? sheet : _defaultSheet;
            }
        }
    }
}
