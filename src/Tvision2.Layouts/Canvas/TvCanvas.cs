using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using Tvision2.Core.Components;
using Tvision2.Core.Engine;
using Tvision2.Core.Render;

namespace Tvision2.Layouts.Canvas
{
    public class TvCanvas : ITvContainer, IComponentsCollection
    {

        private readonly TvComponent<object> _thisComponent;
        private readonly ListComponentCollection _childs;
        private readonly List<TvComponent> _pendingAdds;
        private bool _isMounted;
        public string Name { get; }

        public TvComponent AsComponent() => _thisComponent;

        public IComponentsCollection Children => this;

        public int Count => throw new NotImplementedException();

        int IComponentsCollection.Count => throw new NotImplementedException();
        private ComponentTree _tree;

        public TvCanvas(IComponentTree root, string name = null)
        {
            _isMounted = false;
            _pendingAdds = new List<TvComponent>();
            _childs = new ListComponentCollection();
            _thisComponent = new TvComponent<object>(new Object(), name ?? $"TvCanvas_{Guid.NewGuid()}",
                cfg =>
                {
                    cfg.WhenChildMounted(ctx => OnChildAdded(ctx));
                    cfg.WhenChildUnmounted(ctx => RepositionChildren(TvPoint.Zero));
                    cfg.WhenComponentMounted(ctx => OnComponentMounted(ctx));
                });
            Name = _thisComponent.Name;
            _thisComponent.Metadata.ViewportChanged += OnViewportChange;
        }

        private void OnComponentMounted(ComponentMoutingContext ctx)
        {
            _tree = ctx.ComponentTree;
            _isMounted = true;

            if (_pendingAdds.Any())
            {
                foreach (var cmpToAdd in _pendingAdds)
                {
                    DoPendingAdd(cmpToAdd);
                }
                _pendingAdds.Clear();
                RepositionChildren(TvPoint.Zero);
            }
        }


        private void OnChildAdded(ChildComponentMoutingContext ctx)
        {
            var child = ctx.Child.Metadata;
            InsertChildInside(child.Component);
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
            if (_thisComponent.Viewport != null && _isMounted)
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

        private void DoPendingAdd(TvComponent cmpToAdd)
        {
            _tree.AddAsChild(cmpToAdd, AsComponent(), cfg => cfg.DoNotNotifyParentOnAdd());
        }

        void IComponentsCollection.Add(TvComponent child)
        {
            _childs.Add(child);
            if (_isMounted)
            {
                _tree.AddAsChild(child, AsComponent());
            }
            else
            {
                _pendingAdds.Add(child);
            }
        }


        bool IComponentsCollection.Remove(TvComponent component) => _childs.Remove(component);
        void IComponentsCollection.Clear() => _childs.Clear();

        IEnumerator<TvComponent> IEnumerable<TvComponent>.GetEnumerator() => _childs.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _childs.GetEnumerator();
    }
}
