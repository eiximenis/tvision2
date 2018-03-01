using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Core.Styles
{
    public class StyleBuilder : IStyleBuilder
    {
        private readonly Style _styles;

        public StyleBuilder(IStyle parent = null)
        {
            _styles = new Style(parent);
        }

        public IStyleBuilder WithForegroundColor(ConsoleColor color)
        {
            _styles.ForeColor = color;
            return this;
        }

        public IStyleBuilder WithBackgroundColor(ConsoleColor color)
        {
            _styles.BackColor = color;
            return this;
        }

        public IStyle Build() => _styles;
    }
}
