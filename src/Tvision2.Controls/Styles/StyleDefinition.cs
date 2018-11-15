using Tvision2.Core.Colors;

namespace Tvision2.Controls.Styles
{
    public struct StyleDefinition
    {
        public TvisionColor Foreground { get; set; }
        public TvisionColor Background { get; set; }
        public CharacterAttributeModifiers Attributes { get; set; }
    }
}