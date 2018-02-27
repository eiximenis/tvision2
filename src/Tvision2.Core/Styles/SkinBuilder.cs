using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Core.Styles
{
    public class SkinBuilder : ISkinBuilder
    {
        private readonly Skin _skin;
        public SkinBuilder() => _skin = new Skin();

        public ISkinBuilder UseDefaultStyle(IBaseStyles styles)
        {
            _skin.SetDefaultStyle(styles);
            return this;
        }

        public ISkinBuilder AddStyle(string name, IBaseStyles style)
        {
           if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("must enter name",  nameof(name));
            }

            _skin.AddNamedStyle(name, style);

            return this;
        }

        public ISkinBuilder AddStyle(string name, Action<IBaseStylesBuilder> builderOptions)
        {
            var styleBuilder = new BaseStylesBuilder();
            builderOptions.Invoke(styleBuilder);
            var style = styleBuilder.Build();
            return AddStyle(name, style);
        }

        public ISkin Build() => _skin;

    }
}
