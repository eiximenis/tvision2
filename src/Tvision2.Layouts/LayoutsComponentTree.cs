using System;
using System.Collections.Generic;
using System.Linq;
using Tvision2.Core.Components;
using Tvision2.Core.Engine;

namespace Tvision2.Layouts
{
    internal class LayoutsComponentTree : IComponentTree
    {

        private readonly IComponentTree _root;
        private List<TvComponent> _myComponents;
        public IEnumerable<TvComponent> Components => _myComponents;

        public int Count => _myComponents.Count;

        public TuiEngine Engine => _root.Engine;

        public event EventHandler<TreeUpdatedEventArgs> ComponentAdded;

        public LayoutsComponentTree(IComponentTree root)
        {
            _myComponents = new List<TvComponent>();
            _root = root;
        }

        public IComponentMetadata Add(TvComponent component)
        {
            var metadata = _root.Add(component);
            _myComponents.Add(component);
            OnComponentAdded(component.Metadata);
            return metadata;
        }

        public IComponentMetadata Add(IComponentMetadata metadata)
        {
            _root.Add(metadata);
            _myComponents.Add(metadata.Component);
            OnComponentAdded(metadata);
            return metadata;
        }

        public TvComponent GetComponent(string name)
        {
           return  _myComponents.FirstOrDefault(c => c.Name == name);
        }

        private void OnComponentAdded(IComponentMetadata metadata)
        {
            var handler = ComponentAdded;
            handler?.Invoke(this, new TreeUpdatedEventArgs(metadata));
        }
    }
}
