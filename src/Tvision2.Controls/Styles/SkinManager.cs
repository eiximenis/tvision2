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
        public SkinManager()
        {
            _skins = new Dictionary<string, ISkin>();
            _currentSkin = null;
        }

        public void Fill(Dictionary<string, ISkin> skins)
        {
            foreach (var entry in skins)
            {
                _skins.Add(entry.Key, entry.Value);
            }

            _currentSkin = skins.ContainsKey(string.Empty) ? string.Empty : skins.Keys.First();
        }

        public ISkin GetSkin(string name) => _skins[name];

        public ISkin CurrentSkin => _skins[_currentSkin];
    }

}
