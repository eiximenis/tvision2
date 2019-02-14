using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Engine;

namespace Tvision2.Core.Components
{
    public struct ComponentMoutingContext
    {
        public IComponentTree OwnerTree { get; }
        public TvComponent Component { get; }

        public ComponentMoutingContext(IComponentTree tree, TvComponent component)
        {
            OwnerTree = tree;
            Component = component;
        }
    }
}
