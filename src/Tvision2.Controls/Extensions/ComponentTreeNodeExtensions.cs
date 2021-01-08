using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Tvision2.Core.Engine;

namespace Tvision2.Controls.Extensions
{
    public static class ComponentTreeNodeExtensions
    {
        public static TvControlMetadata? GetParentControl(this ComponentTreeNode node)
        {
            TvControlMetadata? ctl = null;
            var cnode = node.Parent;
            while (cnode != null)
            {
                if (cnode.HasTag<TvControlMetadata>())
                {
                    ctl = cnode.GetTag<TvControlMetadata>();
                    break;
                }
                cnode = cnode.Parent;
            } 

            return ctl;
        }
    }
}
