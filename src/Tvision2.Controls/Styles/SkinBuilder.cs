using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Controls.Styles
{
    public class SkinBuilder : ISkinBuilder
    {
        private readonly Dictionary<string, IStyleSheet> _sheets;
        private IStyleSheet _default;

        public SkinBuilder() => _sheets = new Dictionary<string, IStyleSheet>();

        public ISkinBuilder UseDefaultStyles(IStyleSheet sheet)
        {
            _default = sheet ?? throw new ArgumentNullException(nameof(sheet));
            return this;
        }

        public ISkinBuilder AddStyleSheet(string name, IStyleSheet sheet)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("must enter name", nameof(name));
            }

            _sheets.Add(name, sheet);
            return this;
        }

        public ISkinBuilder AddStyleSheet(string name, Action<IStyleSheetBuilder> builderOptions)
        {
            var builder = new StyleSheetBuilder();
            builderOptions.Invoke(builder);
            var sheet = builder.Build();
            return AddStyleSheet(name, sheet);
        }

        public ISkin Build()
        {
            var skin = new Skin(_sheets);
            if (_default != null)
            {
                skin.SetDefaultStyleSheet(_default);
            }
            return skin;
        }

    }
}
