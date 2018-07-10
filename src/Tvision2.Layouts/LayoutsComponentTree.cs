using System;
using System.Collections.Generic;
using Tvision2.Core.Components;
using Tvision2.Core.Engine;

namespace Tvision2.Layouts
{
    internal class LayoutsComponentTree : IComponentTree
    {

        private readonly ComponentTree _root;
        private List<TvComponent> _myComponents;
        public IEnumerable<TvComponent> Components => _myComponents;

        public event EventHandler<TreeUpdatedEventArgs> ComponentAdded;

        public LayoutsComponentTree(ComponentTree root)
        {
            _myComponents = new List<TvComponent>();
            _root = root;
        }

        public void Add(TvComponent component)
        {
            _root.Add(component);
            _myComponents.Add(component);
        }
    }
}
