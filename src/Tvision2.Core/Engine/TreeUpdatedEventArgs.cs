using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tvision2.Core.Engine
{
    public class TreeUpdatedEventArgs : EventArgs
    {
        public IComponentMetadata ComponentMetadata { get; }

        public TreeUpdatedEventArgs(IComponentMetadata metadata)
        {
            ComponentMetadata = metadata;
        }

    }
}
