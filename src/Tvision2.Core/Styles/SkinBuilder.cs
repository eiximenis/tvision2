using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Core.Styles
{
    public class SkinBuilder : ISkinBuilder
    {
        private readonly Skin _skin;
        public SkinBuilder() => _skin = new Skin();

        public ISkinBuilder UseDefaultStyles(IStyleSheet sheet)
        {
            _skin.SetDefaultStyleSheet(sheet);
            return this;
        }

        public ISkinBuilder AddStyleSheet(string name, IStyleSheet sheet)
        {
           if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("must enter name",  nameof(name));
            }

            _skin.AddNamedStyleSheet(name, sheet);

            return this;
        }

        public ISkinBuilder AddStyleSheet(string name, Action<IStyleSheetBuilder> builderOptions)
        {
            var builder = new StyleSheetBuilder();
            builderOptions.Invoke(builder);
            var sheet = builder.Build();
            return AddStyleSheet(name, sheet);
        }

        public ISkin Build() => _skin;

    }
}
