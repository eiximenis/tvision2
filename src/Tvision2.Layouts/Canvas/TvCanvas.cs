using System;
using System.Collections;
using System.Collections.Generic;
using Tvision2.Core.Components;
using Tvision2.Core.Engine;
using Tvision2.Core.Render;

namespace Tvision2.Layouts.Canvas
{
    public class TvCanvas : ITvContainer, IComponentsCollection
    {

        private readonly TvComponent<object> _thisComponent;
        private readonly ListComponentCollection _childs;
        public string Name { get; }

        public TvComponent AsComponent() => _thisComponent;

        public IComponentsCollection Children => this;

        public int Count => throw new NotImplementedException();

        int IComponentsCollection.Count => throw new NotImplementedException();

        public TvCanvas(IComponentTree root, string name = null)
        {
            _childs = new ListComponentCollection();
            _thisComponent = new TvComponent<object>(new Object(), name ?? $"TvCanvas_{Guid.NewGuid()}");
            Name = _thisComponent.Name;
            _thisComponent.Metadata.ViewportChanged += OnViewportChange;
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

        void IComponentsCollection.Add(TvComponent child) => _childs.Add(child);
        bool IComponentsCollection.Remove(TvComponent component) => _childs.Remove(component);
        void IComponentsCollection.Clear() => _childs.Clear();

        IEnumerator<TvComponent> IEnumerable<TvComponent>.GetEnumerator() => _childs.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _childs.GetEnumerator();
    }
}
