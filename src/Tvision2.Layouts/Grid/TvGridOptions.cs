using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Tvision2.Core.Colors;
using Tvision2.Styles;
using Tvision2.Styles.Backgrounds;

namespace Tvision2.Layouts.Grid
{
    public interface ITvGridOptions
    {
        ITvGridOptions UseDefaultAlignment(ChildAlignment alignment);
        ITvGridOptions UseBorder(BorderValue border, TvColorPair colors);
        ITvGridOptions UseBorder(BorderValue border, IStyle style);
    }

    public class TvGridOptions : ITvGridOptions
    {
        public ChildAlignment DefaultAlignment { get; private set; }
        public BorderValue Border { get; private set; }
        public StyleEntry BorderAttribute { get; private set; }

        public TvGridOptions()
        {
            Border = BorderValue.None();
            DefaultAlignment = ChildAlignment.None;

        }

        ITvGridOptions ITvGridOptions.UseDefaultAlignment(ChildAlignment alignment)
        {
            DefaultAlignment = alignment;
            return this;
        }

        ITvGridOptions ITvGridOptions.UseBorder(BorderValue border, TvColorPair colors)
        {
            Border = border;
            BorderAttribute = new StyleEntry(new SolidColorBackgroundProvider(colors.ForeGround), new SolidColorBackgroundProvider(colors.Background), CharacterAttributeModifiers.Normal);
            return this;
        }


        ITvGridOptions ITvGridOptions.UseBorder(BorderValue border, IStyle style)
        {
            Border = border;
            BorderAttribute = style.Active;
            return this;
        }
    }
}
