using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tvision2.Core.Components;
using Tvision2.Core.Engine;
using Tvision2.Core.Render;

namespace Tvision2.Controls.Window
{
    public class WindowState : IDirtyObject, IComponentsCollection
    {
        private TvWindow _myWindow;
        private readonly ListComponentCollection _children;

        public bool IsDirty { get; private set; }
        public void Validate() => IsDirty = false;

        public IComponentsCollection UI => this;


        public WindowState()
        {
            _children = new ListComponentCollection();
        }

       

        internal void SetOwnerWindow(TvWindow ownerWindow)
        {
            _myWindow = ownerWindow;
            foreach (var existingChild in _children.Components)
            {
                existingChild.UpdateViewport(existingChild.Viewport.InnerViewport(_myWindow.AsComponent().Viewport, TvPoint.FromXY(1,1)));
            }
            IsDirty = true;
        }

        internal void RequestClose()
        {
            _myWindow?.Close();
        }

        int IComponentsCollection.Count => _children.Count;

        void IComponentsCollection.Clear() => _children.Clear();

        void IComponentsCollection.Add(TvComponent child) => _children.Add(child);

        bool IComponentsCollection.Remove(TvComponent component) => _children.Remove(component);

        IEnumerator<TvComponent> IEnumerable<TvComponent>.GetEnumerator() => _children.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _children.GetEnumerator();
    }
}