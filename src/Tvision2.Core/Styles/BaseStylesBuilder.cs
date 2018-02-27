using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Core.Styles
{
    public class BaseStylesBuilder : IBaseStylesBuilder
    {
        private readonly BaseStyles _styles;

        public BaseStylesBuilder(IBaseStyles parent = null)
        {
            _styles = new BaseStyles(parent);
        }

        public IBaseStylesBuilder WithForegroundColor(ConsoleColor color)
        {
            _styles.ForeColor = color;
            return this;
        }

        public IBaseStylesBuilder WithBackgroundColor(ConsoleColor color)
        {
            _styles.BackColor = color;
            return this;
        }

        public IBaseStyles Build() => _styles;
    }
}
