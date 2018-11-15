using Tvision2.Core.Colors;

namespace Tvision2.Controls.Styles
{
    public struct StyleDefinition
    {
        public TvColor Foreground { get; set; }
        public TvColor Background { get; set; }
        public CharacterAttributeModifiers Attributes { get; set; }
    }
}