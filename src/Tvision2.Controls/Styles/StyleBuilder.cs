using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using Tvision2.Core.Colors;

namespace Tvision2.Controls.Styles
{
    class StyleBuilder : IStyleBuilder
    {
        public readonly Style _style;

        private readonly IDictionary<string, StyleDefinition> _customDefinitions;
        private StyleDefinition _standard;
        private StyleDefinition _focused;
        private StyleDefinition _alternate;
        private StyleDefinition _alternateFocused;

        public StyleBuilder()
        {
            _style = new Style();
            _customDefinitions = new Dictionary<string, StyleDefinition>();
        }

        public IStyleBuilder DesiredFocused(TvColor fore, TvColor back,
            CharacterAttributeModifiers attributes = CharacterAttributeModifiers.Normal)
        {
            _focused = new StyleDefinition()
            {
                Foreground = fore,
                Background = back,
                Attributes = attributes
            };
            return this;
        }

        public IStyleBuilder DesiredStandard(TvColor fore, TvColor back,
            CharacterAttributeModifiers attributes = CharacterAttributeModifiers.Normal)
        {
            _standard = new StyleDefinition()
            {
                Foreground = fore,
                Background = back,
                Attributes = attributes
            };
            return this;
        }

        public IStyleBuilder DesiredAlternate(TvColor fore, TvColor back,
            CharacterAttributeModifiers attributes = CharacterAttributeModifiers.Normal)
        {
            _alternate = new StyleDefinition()
            {
                Foreground = fore,
                Background = back,
                Attributes = attributes
            };
            return this;
        }

        public IStyleBuilder DesiredAlternateFocused(TvColor fore, TvColor back,
            CharacterAttributeModifiers attributes = CharacterAttributeModifiers.Normal)
        {
            _alternateFocused = new StyleDefinition()
            {
                Foreground = fore,
                Background = back,
                Attributes = attributes
            };
            return this;
        }

        public IStyleBuilder DesiredCustom(string name, TvColor fore, TvColor back,
            CharacterAttributeModifiers attributes = CharacterAttributeModifiers.Normal)
        {
            _customDefinitions.Add(name, new StyleDefinition()
            {
                Foreground = fore,
                Background = back,
                Attributes = attributes
            });
            return this;
        }

        public IStyle Build(IColorManager colorManager)
        {
            _style.Standard =
                colorManager.BuildAttributeFor(_standard.Foreground, _standard.Background, _standard.Attributes);
            _style.Focused = colorManager.BuildAttributeFor(_focused.Foreground, _focused.Background, _focused.Attributes);
            _style.Alternate = colorManager.BuildAttributeFor(_alternate.Foreground, _alternate.Background, _alternate.Attributes);
            _style.AlternateFocused = colorManager.BuildAttributeFor(_alternateFocused.Foreground, _alternateFocused.Background, _alternateFocused.Attributes);
            foreach (var custom in _customDefinitions)
            {
                _style.SetupCustomValue(custom.Key,
                    colorManager.BuildAttributeFor(custom.Value.Foreground, custom.Value.Background, custom.Value.Attributes));
            }

            return _style;
        }
    }
}