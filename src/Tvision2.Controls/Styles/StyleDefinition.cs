using Tvision2.Core.Colors;

namespace Tvision2.Controls.Styles
{
    public struct StyleDefinition
    {
        public DefaultColorName Foreground { get; set; }
        public DefaultColorName Background { get; set; }
        public CharacterAttributeModifiers Attributes { get; set; }
    }
}