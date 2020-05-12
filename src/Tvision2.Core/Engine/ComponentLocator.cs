using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Components;

namespace Tvision2.Core.Engine
{
    public readonly ref struct ComponentLocator
    {
        private readonly ComponentTree _tree;
        private readonly ComponentTreeNode _parent;
        public ComponentLocator(ComponentTree tree, ComponentTreeNode parentNode)
        {
            _tree = tree;
            _parent = parentNode;
        }

        public TvComponent GetParent() => _parent.Data.Component;

        TvComponent<TState> GetParent<TState>() => GetParent() as TvComponent<TState>;

        TvComponent GetRoot() => _parent.Root.Data.Component;

        TvComponent<TState> GetRoot<TState>() => _parent.Root.Data.Component as TvComponent<TState>;


        TvComponent GetByName(string name) => _tree.GetComponent(name);
        TvComponent<TState> GetByName<TState>(string name) => GetByName(name) as TvComponent<TState>;
    }
}
