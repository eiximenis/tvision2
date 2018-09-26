using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Colors;

namespace Tvision2.Controls.Styles
{
    internal class SkinManagerBuilder : ISkinManagerBuilder
    {
        private readonly Dictionary<string, ISkin> _skins;
        private readonly IColorManager _colorManager;

        public SkinManagerBuilder(IColorManager colorManager)
        {
            _skins = new Dictionary<string, ISkin>();
            _colorManager = colorManager;
        }

        public ISkinManagerBuilder AddSkin(string name, Action<ISkinBuilder> builderOptions = null)
        {
            var skinBuilder = new SkinBuilder(_colorManager);
            builderOptions?.Invoke(skinBuilder);
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
