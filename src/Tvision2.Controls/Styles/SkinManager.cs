using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tvision2.Controls.Styles
{
    class SkinManager : ISkinManager
    {
        private string _currentSkin;

        private readonly Dictionary<string, ISkin> _skins;
        public SkinManager(Dictionary<string, ISkin> skins)
        {
            _skins = skins;
            _currentSkin = skins.ContainsKey(string.Empty) ? string.Empty : skins.Keys.First();
        }

        public ISkin GetSkin(string name) => _skins[name];

        public ISkin CurrentSkin => _skins[_currentSkin];
    }

}
