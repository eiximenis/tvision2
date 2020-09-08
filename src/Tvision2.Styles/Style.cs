using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using Tvision2.Core.Colors;

namespace Tvision2.Styles
{
    class Style : IStyle
    {
        private readonly Dictionary<string, StyleEntry> _customValues;

        public Style()
        {
            _customValues = new Dictionary<string, StyleEntry>();
        }

        public StyleEntry Standard { get; internal set; }
        public StyleEntry Active { get; internal set; }
        public StyleEntry Alternate { get; internal set; }
        public StyleEntry AlternateActive { get; internal set; }
        public StyleEntry this[string name] => _customValues.TryGetValue(name, out StyleEntry result) ? result : Standard;
        internal void SetupCustomValue(string name, StyleEntry attr) => _customValues.Add(name, attr);

        public void Mix(Style styleDelta)
        {
            Standard.Mix(styleDelta.Standard);
            Active.Mix(styleDelta.Active);
            Alternate.Mix(styleDelta.Alternate);
            AlternateActive.Mix(styleDelta.AlternateActive);

            
            foreach (var entry in styleDelta._customValues)
            {
                if (_customValues.ContainsKey(entry.Key))
                {
                    _customValues[entry.Key].Mix(entry.Value);
                }
                else
                {
                    _customValues.Add(entry.Key, entry.Value);
                }
            }
        }
    }
}