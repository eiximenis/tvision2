using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Core.Styles
{
    class Skin : ISkin
    {
        private readonly Dictionary<string, IStyleSheet> _styleSheets;
        private IStyleSheet _defaultSheet;

        public Skin()
        {
            _styleSheets = new Dictionary<string, IStyleSheet>();
            _defaultSheet = StyleSheet.Default;
        }

        internal void AddNamedStyleSheet(string name, IStyleSheet sheet) => _styleSheets.Add(name, sheet);

        internal void SetDefaultStyleSheet(IStyleSheet sheet) => _defaultSheet = sheet;

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
