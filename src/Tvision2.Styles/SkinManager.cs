using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tvision2.Styles
{
    class SkinManager : ISkinManager
    {
        private string DEFAULT_SKIN_KEY = "Default";

        private string _defaultSkinKey;

        private string _currentSkinKey;

        private readonly Dictionary<string, ISkin> _skins;
        public SkinManager()
        {
            _skins = new Dictionary<string, ISkin>();
            _currentSkinKey = null;
            _defaultSkinKey = null;
        }

        public void Fill(Dictionary<string, ISkin> skins)
        {
            foreach (var entry in skins)
            {
                _skins.Add(entry.Key, entry.Value);
            }

            _defaultSkinKey = skins.ContainsKey(DEFAULT_SKIN_KEY) ? DEFAULT_SKIN_KEY : skins.Keys.First();
            _currentSkinKey = _defaultSkinKey;
        }

        public ISkin GetDefaultSkin() => _skins[_defaultSkinKey];
        public ISkin CurrentSkin => _skins[_currentSkinKey];

        public ISkin GetSkin(string name) => _skins[name];

        public ISkinManager ChangeCurrentSkin(string key)
        {
            if (_skins.ContainsKey(key))
            {
                _defaultSkinKey = key;
            }
            else
            {
                throw new InvalidOperationException($"Skin {key} do not exist");
            }
            
            return this;
        }

        public IEnumerable<string> SkinNames
        {
            get => _skins.Keys;
        }
    }

}
