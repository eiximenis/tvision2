using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tvision2.Styles;

namespace Tvision2.Layouts.Grid
{
    public interface ITvGridOptions
    {
        ITvGridOptions UseDefaultAlignment(ChildAlignment alignment);
        ITvGridOptions UseBorder(BorderValue border);

    }

    public class TvGridOptions : ITvGridOptions 
    {
        public ChildAlignment DefaultAlignment { get; private set; }
        public BorderValue Border { get; private set; }

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

        ITvGridOptions ITvGridOptions.UseBorder(BorderValue border)
        {
            Border = border;
            return this;
        }


    }
}
