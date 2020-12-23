using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tvision2.Layouts.Grid
{
    public interface ITvGridOptions
    {
        ITvGridOptions UseDefaultAlignment(ChildAlignment alignment);
        ITvGridOptions UseBorder();          // TODO: Change by border style (thin, thick... lets see)

    }

    public class TvGridOptions : ITvGridOptions 
    {
        public ChildAlignment DefaultAlignment { get; private set; }
        public bool HasBorder { get; private set; }

        public TvGridOptions()
        {
            HasBorder = false;
            DefaultAlignment = ChildAlignment.None;
        }

        ITvGridOptions ITvGridOptions.UseDefaultAlignment(ChildAlignment alignment)
        {
            DefaultAlignment = alignment;
            return this;
        }

        ITvGridOptions ITvGridOptions.UseBorder()
        {
            HasBorder = true;
            return this;
        }


    }
}
