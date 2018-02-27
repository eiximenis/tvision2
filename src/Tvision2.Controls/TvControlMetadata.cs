using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Engine;

namespace Tvision2.Controls
{
    public class TvControlMetadata
    {
        private readonly TvComponentMetadata _metadata;
        public ITvControl Control { get; }

        public TvControlMetadata(TvComponentMetadata cdata, ITvControl control)
        {
            _metadata = cdata;
            Control = control;
        }
    }
}
