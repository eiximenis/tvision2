using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tvision2.Core.Colors;

namespace Tvision2.Styles
{
    internal class SkinManagerBuilder : ISkinManagerBuilder
    {
        private readonly Dictionary<string, SkinBuilder> _skinsToBuild;
        private readonly IColorManager _colorManager;

        public SkinManagerBuilder(IColorManager colorManager)
        {
            _skinsToBuild = new Dictionary<string, SkinBuilder>();
            _colorManager = colorManager;
        }

        public ISkinManagerBuilder AddDefaultSkin(Action<ISkinBuilder> builderOptions) => AddSkin("Default", builderOptions);

        public ISkinManagerBuilder AddSkin(string name, Action<ISkinBuilder> builderOptions = null)
        {
            var skinBuilder = new SkinBuilder();
            builderOptions?.Invoke(skinBuilder);
            AddSkinToBuild(name, skinBuilder);
            return this;
        }

        public void Fill(SkinManager skinManager)
        {
            var skins = _skinsToBuild.ToDictionary(k => k.Key, k => k.Value.Build(_colorManager));
            skinManager.Fill(skins);
        }

        private void AddSkinToBuild(string name, SkinBuilder skinToBuild)
        {
            if (_skinsToBuild.ContainsKey(name))
            {
                throw new ArgumentException($"Style name {name} duplicated.", nameof(name));
            }

            _skinsToBuild.Add(name, skinToBuild);
        }
    }
}
