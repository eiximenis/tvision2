using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tvision2.Core.Engine
{
    public class TreeUpdatedEventArgs : EventArgs
    {
        public TvComponentMetadata ComponentMetadata { get; }
        public ComponentTreeNode Node { get; }

        public TreeUpdatedEventArgs(TvComponentMetadata metadata, ComponentTreeNode node)
        {
            ComponentMetadata = metadata;
            Node = node;
        }

    }
}
