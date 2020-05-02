using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using Tvision2.Core.Colors;

namespace Tvision2.Controls.Styles
{
    class StyleEntryBuilder : IStyleEntryBuilder
    {
        public readonly Style _style;

        private readonly IDictionary<string, StyleEntry> _customDefinitions;
        private StyleEntry _standard;
        private StyleEntry _focused;
        private StyleEntry _alternate;
        private StyleEntry _alternateFocused;

        public StyleEntryBuilder()
        {
            _style = new Style();
            _customDefinitions = new Dictionary<string, StyleEntry>();
        }


        public Style Build()
        {
            _style.Standard = _standard;
            _style.Focused = _focused ?? _standard;
            _style.Alternate = _alternate ?? _standard;
            _style.AlternateFocused = _alternateFocused ?? _standard;
            foreach (var custom in _customDefinitions)
            {
                _style.SetupCustomValue(custom.Key, custom.Value);
            }

            return _style;
        }

        public IStyleEntryBuilder DesiredStandard(Action<StyleDefintionOptions> optionsAction)
        {
            _standard = BuildDefinition(optionsAction);
            return this;
        }

        private StyleEntry BuildDefinition(Action<StyleDefintionOptions> optionsAction)
        {
            var options = new StyleDefintionOptions();
            optionsAction.Invoke(options);
            return new StyleEntry(options.ForeColor, options.Background, options.Modifiers);
        }

        public IStyleEntryBuilder DesiredFocused(Action<StyleDefintionOptions> optionsAction)
        {
            _focused = BuildDefinition(optionsAction);
            return this;
        }

        public IStyleEntryBuilder DesiredAlternate(Action<StyleDefintionOptions> optionsAction)
        {
            _alternate = BuildDefinition(optionsAction);
            return this;
        }

        public IStyleEntryBuilder DesiredAlternateFocused(Action<StyleDefintionOptions> optionsAction)
        {
            _alternateFocused = BuildDefinition(optionsAction);
            return this;
        }

        public IStyleEntryBuilder DesiredCustom(string name, Action<StyleDefintionOptions> optionsAction)
        {
            var def = BuildDefinition(optionsAction);
            _customDefinitions.Add(name, def);
            return this;
        }
    }
}