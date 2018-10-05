using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using Tvision2.Core.Colors;

namespace Tvision2.Controls.Styles
{
    class Style : IStyle
    {
        private readonly Dictionary<string, CharacterAttribute> _customValues;

        public Style()
        {
            _customValues = new Dictionary<string, CharacterAttribute>();
        }

        public CharacterAttribute Standard { get; internal set; }
        public CharacterAttribute Focused { get; internal set; }
        public CharacterAttribute Alternate { get; internal set; }
        public CharacterAttribute AlternateFocused { get; internal set; }
        public CharacterAttribute this[string name] => _customValues.TryGetValue(name, out CharacterAttribute result) ? result : Standard;
        internal void SetupCustomValue(string name, CharacterAttribute attr) => _customValues.Add(name, attr);
    }
}