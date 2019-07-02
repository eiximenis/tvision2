using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tvision2.Core;
using Tvision2.Core.Colors;

namespace Tvision2.Controls.Styles
{
    public class StyleBuilder : IStyleBuilder
    {

        private StyleEntryBuilder _defaultBuilder;
        private readonly List<(Func<IColorManager, bool> predicate, StyleEntryBuilder builder)> _builders;

        public StyleBuilder()
        {
            _defaultBuilder = new StyleEntryBuilder();
            _builders = new List<(Func<IColorManager, bool>, StyleEntryBuilder)>();
        }


        public IStyleEntryBuilder Default()
        {
            return _defaultBuilder;
        }

        public IStyleEntryBuilder When(Func<ColorMode, bool> colorModePredicate)
        {
            var builder = new StyleEntryBuilder();
            Func<IColorManager, bool> predicate = cm => colorModePredicate(cm.Palette.ColorMode);
            _builders.Add((predicate, builder));
            return builder;
        }

        public IStyleEntryBuilder When(Func<IPalette, bool> palettePredicate)
        {
            var builder = new StyleEntryBuilder();
            Func<IColorManager, bool> predicate = cm => palettePredicate(cm.Palette);
            _builders.Add((predicate, builder));
            return builder;
        }

        public IStyle Build(IColorManager cm)
        {
            var style = _defaultBuilder.Build();

            foreach (var builder in _builders.Where(b => b.predicate(cm)).Select(b => b.builder))
            {
                var styleDelta = builder.Build();
                style.Mix(styleDelta);
            }
            return style;
        }
    }
}
