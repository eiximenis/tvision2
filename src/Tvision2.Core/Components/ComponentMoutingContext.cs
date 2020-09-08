using Microsoft.Extensions.FileProviders.Physical;
using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Engine;

namespace Tvision2.Core.Components
{
    public struct ComponentMoutingContext
    {
        public ITuiEngine OwnerEngine { get; }
        public TvComponent Component { get; }

        public ComponentTreeNode Node { get; }

        public ComponentTree ComponentTree { get; }

        public ComponentMoutingContext(ITuiEngine ownerEngine, ComponentTree ctree, TvComponent component, ComponentTreeNode node)
        {
            OwnerEngine = ownerEngine;
            Component = component;
            Node = node;
            ComponentTree = ctree;
        }
    }

    public struct ChildComponentMoutingContext
    {
        public TvComponent Child { get; }
        public ComponentTreeNode ChildNode { get; }

        public ChildComponentMoutingContext(TvComponent child, ComponentTreeNode childNode)
        {
            Child = child;
            ChildNode = childNode;
        }
    }

    public struct ComponentMountingCancellableContext
    {
        public ITuiEngine OwnerEngine { get; }
        public TvComponent Component { get; }

        public bool IsCancelled { get; private set; }

        public bool Cancel() => IsCancelled = true;

        public ComponentMountingCancellableContext(ITuiEngine ownerEngine, TvComponent component)
        {
            OwnerEngine = ownerEngine;
            Component = component;
            IsCancelled = false;
        }

    }

    public struct ComponentTreeUpdatedContext
    {
        public ComponentTree ComponentTree { get; }
        public TvComponent Component { get; }
        public ComponentTreeUpdatedContext(ComponentTree componentTree, TvComponent component)
        {
            ComponentTree = componentTree;
            Component = component;
        }
    }
}
