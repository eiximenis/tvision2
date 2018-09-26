using System;
using System.Collections.Generic;
using Tvision2.Core.Colors;

namespace Tvision2.Controls.Styles
{
    public class SkinBuilder : ISkinBuilder
    {
        private readonly Dictionary<string, IStyle> _styles;
        private const string DEFAULT_STYLE_NAME = "";
        private readonly IColorManager _colorManager;

        public SkinBuilder(IColorManager colorManager)
        {
            _styles = new Dictionary<string, IStyle>();
            _colorManager = colorManager;
        }




        public ISkin Build()
        {
            var skin = new Skin(_styles);
            return skin;
        }

        public ISkinBuilder AddBaseStyle(Action<IStyleBuilder> builderOptions) => AddStyle(DEFAULT_STYLE_NAME, builderOptions);

        public ISkinBuilder AddStyle(string name, Action<IStyleBuilder> builderOptions)
        {
            var builder = new StyleBuilder();
            builderOptions.Invoke(builder);
            var style = builder.Build(_colorManager);
            _styles.Add(name, style);
            return this;
        }

    }
}
