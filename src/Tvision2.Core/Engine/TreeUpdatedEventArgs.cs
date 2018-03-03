using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Core.Engine
{
    public class TreeUpdatedEventArgs : EventArgs
    {
        public TvComponentMetadata ComponentMetadata { get; }
        public TreeUpdatedEventArgs(TvComponentMetadata metadata)
        {
            ComponentMetadata = metadata;
        }

    }
}
