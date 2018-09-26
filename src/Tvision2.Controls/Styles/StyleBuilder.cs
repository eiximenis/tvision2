using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Colors;

namespace Tvision2.Controls.Styles
{
    class StyleBuilder : IStyleBuilder
    {

        public readonly Style _style;
        private (DefaultColorName fore, DefaultColorName back) _standard;
        private (DefaultColorName fore, DefaultColorName back) _focused;

        public StyleBuilder()
        {
            _style = new Style();
        }
        public IStyleBuilder DesiredFocused(DefaultColorName fore, DefaultColorName back)
        {
            _focused = (fore, back);
            return this;
        }

        public IStyleBuilder DesiredStandard(DefaultColorName fore, DefaultColorName back)
        {
            _standard = (fore, back);
            return this;
        }

        public IStyle Build(IColorManager colorManager)
        {
            _style.Standard = colorManager.GetPairIndexFor(_standard.fore, _standard.back);
            _style.Focused = colorManager.GetPairIndexFor(_focused.fore, _focused.back);
            return _style;
        }
    }
}
