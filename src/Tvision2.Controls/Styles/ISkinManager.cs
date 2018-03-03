using Tvision2.Core.Render;

namespace Tvision2.Controls.Styles
{
    public interface ISkinManager
    {
        ISkin GetSkin(string name);
        ISkin CurrentSkin { get; }
    }
}