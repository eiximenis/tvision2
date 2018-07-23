using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Controls.Styles
{
    public class StyleBuilder : IStyleBuilder
    {
        private readonly Style _style;

        public StyleBuilder(IStyle parent = null)
        {
            _style = new Style(parent);
        }

        public IStyleBuilder WithForegroundColor(ConsoleColor color)
        {
            _style.ForeColor = color;
            return this;
        }

        public IStyleBuilder WithBackgroundColor(ConsoleColor color)
        {
            _style.BackColor = color;
            return this;
        }

        public IStyle Build() => _style;
    }
}
