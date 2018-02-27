using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Core.Styles
{
    class SkinManagerBuilder : ISkinManagerBuilder
    {
        private readonly Dictionary<string, ISkin> _skins;

        public SkinManagerBuilder()
        {
            _skins = new Dictionary<string, ISkin>();
        }

        public ISkinManagerBuilder AddSkin(string name, Action<ISkinBuilder> builderOptions)
        {
            if (builderOptions == null)
            {
                throw new ArgumentNullException(nameof(builderOptions));
            }

            var skinBuilder = new SkinBuilder();
            builderOptions.Invoke(skinBuilder);
            var skin = skinBuilder.Build();
            return AddSkin(name, skin);
        }

        public ISkinManager Build()
        {
            return new SkinManager(_skins);
        }

        public ISkinManagerBuilder AddSkin(string name, ISkin skin)
        {
            if (_skins.ContainsKey(name))
            {
                throw new ArgumentException($"Style name {name} duplicated.", nameof(name));
            }

            _skins.Add(name, skin);
            return this;
        }
    }
}
