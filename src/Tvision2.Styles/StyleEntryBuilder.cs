using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using Tvision2.Core.Colors;

namespace Tvision2.Styles
{
    class StyleEntryBuilder : IStyleEntryBuilder, IStyleAdditionalEntriesBuilder
    {
        public readonly Style _style;

        private readonly IDictionary<string, StyleEntry> _customDefinitions;
        private StyleEntry _standard;
        private StyleEntry _active;
        private StyleEntry _alternate;
        private StyleEntry _alternateActive;

        public StyleEntryBuilder()
        {
            _customDefinitions = new Dictionary<string, StyleEntry>();
        }


        public Style Build()
        {
            var style = new Style();
            style.Standard = _standard;
            style.Active = _active ?? _standard;
            style.Alternate = _alternate ?? _standard;
            style.AlternateActive = _alternateActive ?? style.Active;
            foreach (var custom in _customDefinitions)
            {
                style.SetupCustomValue(custom.Key, custom.Value);
            }

            return style;
        }

        public Style BuildDelta()
        {
            var delta = new Style();
            delta.Standard = _standard;
            delta.Active = _active;
            delta.Alternate = _alternate;
            delta.AlternateActive = _alternateActive;
            foreach (var custom in _customDefinitions)
            {
                delta.SetupCustomValue(custom.Key, custom.Value);
            }

            return delta;
        }

        public IStyleAdditionalEntriesBuilder DesiredStandard(Action<StyleDefintionOptions> optionsAction)
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

        public IStyleAdditionalEntriesBuilder DesiredFocused(Action<StyleDefintionOptions> optionsAction)
        {
            _active = BuildDefinition(optionsAction);
            return this;
        }

        public IStyleAdditionalEntriesBuilder DesiredAlternate(Action<StyleDefintionOptions> optionsAction)
        {
            _alternate = BuildDefinition(optionsAction);
            return this;
        }

        public IStyleAdditionalEntriesBuilder DesiredAlternateFocused(Action<StyleDefintionOptions> optionsAction)
        {
            _alternateActive = BuildDefinition(optionsAction);
            return this;
        }

        public IStyleAdditionalEntriesBuilder Desired(string name, Action<StyleDefintionOptions> optionsAction)
        {
            var def = BuildDefinition(optionsAction);
            _customDefinitions.Add(name, def);
            return this;
        }
    }
}