using System;
using System.Collections.Generic;
using System.Linq;
using Tvision2.Core.Components;
using Tvision2.Core.Engine;
using Tvision2.Core.Render;

namespace Tvision2.Controls.Window
{
    public class WindowState : IDirtyObject, IComponentTree
    {
        private readonly IComponentTree _ownerTree;
        private readonly List<TvComponent> _children;
        private TvWindow _myWindow;

        public bool IsDirty { get; private set; }
        public void Validate() => IsDirty = false;

        public WindowState(IComponentTree ownerTree)
        {
            _ownerTree = ownerTree;
            _children = new List<TvComponent>();
        }
        
        internal void SetOwnerWindow(TvWindow ownerWindow)
        {
            _myWindow = ownerWindow;
            foreach (var existingChild in _children)
            {
                existingChild.UpdateViewport(existingChild.Viewport.InsertInto(_myWindow.Viewport));
            }
        }

        public TuiEngine Engine => _ownerTree.Engine;

        public IEnumerable<TvComponent> Components => _children;

        public event EventHandler<TreeUpdatedEventArgs> ComponentAdded;

        public IComponentMetadata Add(TvComponent component)
        {
            _children.Add(component);
            if (_myWindow != null)
            {
                component.UpdateViewport(component.Viewport.InsertInto(_myWindow.Viewport));
            }
            _ownerTree.Add(component);
            OnComponentAdded(component.Metadata);
            return component.Metadata;
        }

        public IComponentMetadata Add(IComponentMetadata metadata) => Add(metadata.Component);

        public TvComponent GetComponent(string name) => _children.FirstOrDefault(cm => cm.Name == name);

        private void OnComponentAdded(IComponentMetadata metadata)
        {
            ComponentAdded?.Invoke(this, new TreeUpdatedEventArgs(metadata));
        }


    }
}