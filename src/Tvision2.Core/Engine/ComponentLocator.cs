using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tvision2.Core.Components;

namespace Tvision2.Core.Engine
{
    public readonly ref struct ComponentLocator
    {
        private readonly ComponentTree _tree;
        private readonly ComponentTreeNode _parent;
        private readonly ComponentTreeNode _node;
        public ComponentLocator(ComponentTree tree, ComponentTreeNode node)
        {
            _tree = tree;
            _parent = node.Parent;
            _node = node;
        }

        public TvComponent GetParent() => _parent.Data.Component;

        public TvComponent<TState> GetParent<TState>() => GetParent() as TvComponent<TState>;

        public ComponentTreeNode GetParentNode() => _parent;

        public TvComponent GetRoot() => _parent.Root.Data.Component;

        public TvComponent<TState> GetRoot<TState>() => _parent.Root.Data.Component as TvComponent<TState>;


        public IEnumerable<ComponentTreeNode> DescendantNodes() => _node.Descendants();
        public IEnumerable<TvComponentMetadata> Descendants() => _node.Descendants().Select(n => n.Data);

        public TvComponent GetByName(string name) => _tree.GetComponent(name);
        public TvComponent<TState> GetByName<TState>(string name) => GetByName(name) as TvComponent<TState>;
    }
}
