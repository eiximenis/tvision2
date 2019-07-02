using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using Tvision2.Core.Colors;

namespace Tvision2.Controls.Styles
{
    class Style : IStyle
    {
        private readonly Dictionary<string, StyleEntry> _customValues;

        public Style()
        {
            _customValues = new Dictionary<string, StyleEntry>();
        }

        public StyleEntry Standard { get; internal set; }
        public StyleEntry Focused { get; internal set; }
        public StyleEntry Alternate { get; internal set; }
        public StyleEntry AlternateFocused { get; internal set; }
        public StyleEntry this[string name] => _customValues.TryGetValue(name, out StyleEntry result) ? result : Standard;
        internal void SetupCustomValue(string name, StyleEntry attr) => _customValues.Add(name, attr);
    }
}