using System;
using System.Collections.Generic;
using System.Linq;
using Tvision2.Core.Colors;

namespace Tvision2.Controls.Styles
{
    public class SkinBuilder : ISkinBuilder
    {
        private readonly Dictionary<string, StyleBuilder> _stylesToBuild;
        private const string DEFAULT_STYLE_NAME = "";

        public SkinBuilder()
        {
            _stylesToBuild = new Dictionary<string, StyleBuilder>();
        }

        public ISkin Build(IColorManager cm)
        {

            var styles = _stylesToBuild.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Build(cm));
            var skin = new Skin(styles, cm);
            return skin;
        }

        public ISkinBuilder AddBaseStyle(Action<IStyleBuilder> builderOptions) => AddStyle(DEFAULT_STYLE_NAME, builderOptions);

        public ISkinBuilder AddStyle(string name, Action<IStyleBuilder> builderOptions)
        {
            var builder = new StyleBuilder();
            builderOptions.Invoke(builder);
            _stylesToBuild.Add(name, builder);
            return this;
        }

    }
}
