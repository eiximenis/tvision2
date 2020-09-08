using System.Collections.Generic;
using Tvision2.Core.Render;

namespace Tvision2.Styles
{
    public interface ISkinManager
    {

        ISkin GetDefaultSkin();
        ISkin GetSkin(string name);
        ISkin CurrentSkin { get; }
        ISkinManager ChangeCurrentSkin(string key);
        IEnumerable<string> SkinNames { get; }
    }
}