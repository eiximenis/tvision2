using System;
using System.Collections.Generic;
using System.Linq;
using Tvision2.Core.Components;
using Tvision2.Core.Engine;
using Tvision2.Core.Render;

namespace Tvision2.Controls.Window
{
    public class WindowState : IDirtyObject
    {
        private TvWindow _myWindow;
        private readonly ListComponentTree _children;

        public bool IsDirty { get; private set; }
        public void Validate() => IsDirty = false;

        public IComponentTree UI => _children;

        public WindowState(IComponentTree ownerTree)
        {
            _children = new ListComponentTree(ownerTree);
        }
        
        internal void SetOwnerWindow(TvWindow ownerWindow)
        {
            _myWindow = ownerWindow;
            foreach (var existingChild in _children.Components)
            {
                existingChild.UpdateViewport(existingChild.Viewport.InnerViewport(_myWindow.Viewport, new TvPoint(1,1)));
            }
            IsDirty = true;
        }

        internal void RequestClose()
        {
            _myWindow?.Close();
        }

    }
}