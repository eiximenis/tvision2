﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Controls.Styles
{
    class StyleSheetBuilder : IStyleSheetBuilder
    {

        private readonly Dictionary<string, IStyle> _styles;

        public StyleSheetBuilder()
        {
            _styles = new Dictionary<string, IStyle>();
        }

        public IStyleSheetBuilder AddClass(string name, IStyle style)
        {
            _styles.Add(name, style);
            return this;
        }

        public IStyleSheetBuilder AddClass(string name, string baseName, Action<IStyleBuilder> action)
        {
            IStyle parent = null;
            if (!string.IsNullOrEmpty(baseName))
            {
                parent = _styles[baseName];
            }
            var builder = new StyleBuilder(parent);
            action.Invoke(builder);
            var style = builder.Build();
            _styles.Add(name, style);
            return this;
        }

        public IStyleSheetBuilder AddClass(string name, Action<IStyleBuilder> action) => AddClass(name, null, action);


        public IStyleSheet Build() => new StyleSheet(_styles);
    }
}
