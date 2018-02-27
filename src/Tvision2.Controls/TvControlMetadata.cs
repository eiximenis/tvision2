using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Engine;

namespace Tvision2.Controls
{
    public class TvControlMetadata
    {
        public ITvControl Control { get; }

        internal TvComponentMetadata ComponentMetadata { get; }

        public TvControlMetadata(TvComponentMetadata cdata, ITvControl control)
        {
            ComponentMetadata = cdata;
            Control = control;
        }
    }
}
