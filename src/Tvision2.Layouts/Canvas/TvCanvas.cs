﻿using System;
using Tvision2.Core.Components;
using Tvision2.Core.Engine;
using Tvision2.Core.Render;

namespace Tvision2.Layouts.Canvas
{
    public class TvCanvas : ITvContainer
    {

        private readonly TvComponent<object> _thisComponent;
        private readonly ListComponentTree _childs;
        private readonly IComponentTree _root;
        public string Name { get; }

        public TvComponent AsComponent() => _thisComponent;

        public IComponentTree Children => _childs;

        public TvCanvas(IComponentTree root, string name = null)
        {
            _root = root;
            _childs = new ListComponentTree(root);
            _thisComponent = new TvComponent<object>(new Object(), name ?? $"TvCanvas_{Guid.NewGuid()}");
            Name = _thisComponent.Name;
            _thisComponent.Metadata.ViewportChanged += OnViewportChange;
            _childs.ComponentAdded += OnChildAdded;
            _childs.ComponentRemoved += OnChildRemoved;
            root.ComponentRemoved += OnRootRemoved;
        }

        private void OnRootRemoved(object sender, TreeUpdatedEventArgs e)
        {
            if (e.ComponentMetadata == _thisComponent.Metadata)
            {
                Clear();
                _root.ComponentAdded -= OnRootRemoved;
            }
        }

        public void Clear()
        {
            _childs.Clear();
        }

        private void OnChildRemoved(object sender, TreeUpdatedEventArgs e)
        {
            RepositionChildren(TvPoint.Zero);
        }

        private void OnChildAdded(object sender, TreeUpdatedEventArgs e)
        {
            var child = e.ComponentMetadata.Component;
            InsertChildInside(child);
            RepositionChildren(TvPoint.Zero);
        }


        private void InsertChildInside(TvComponent child)
        {
            //if (_thisComponent.Viewport != null)
            //{
            //    child.UpdateViewport(_thisComponent.Viewport.Inner(child.Viewport), addIfNotExists: true);
            //}
        }


        private void RepositionChildren(TvPoint displacement)
        {
            if (_thisComponent.Viewport != null)
            {
                foreach (var child in _childs.Components)
                {
                    var childvp = child.Viewport;
                    child.UpdateViewport(childvp.Translate(_thisComponent.Viewport.Position));
                }
            }
        }

        private void OnViewportChange(object sender, ViewportUpdatedEventArgs e)
        {
            if (e.Previous == Viewport.NullViewport)
            {
                foreach (var child in _childs.Components)
                {
                    RepositionChildren(TvPoint.Zero);
                }
            }
            else
            {
                var displacement = e.Current.Position - e.Previous.Position;
                RepositionChildren(displacement);
            }
        }

    }
}
