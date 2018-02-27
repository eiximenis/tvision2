using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Core.Styles
{
    class SkinManager : ISkinManager
    {
        private readonly Dictionary<string, ISkin> _skins;
        public SkinManager(Dictionary<string, ISkin> skins) => _skins = skins;

        public ISkin GetSkin(string name) => _skins[name];
    }
}
