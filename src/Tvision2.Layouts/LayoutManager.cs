using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tvision2.Core.Engine;
using Tvision2.Engine.Console;
using Tvision2.Viewports;

namespace Tvision2.Layouts
{
    class LayoutManager : ILayoutManager
    {
        private Dictionary<string, ITvContainer> _containers;
        public IViewportFactory ViewportFactory { get; }
        public IDynamicViewportFactory DynamicViewportFactory { get; }
        public LayoutManager(IViewportFactory viewportFactory, IDynamicViewportFactory dynamicViewportFactory, ITuiEngine engine)
        {
            _containers = new Dictionary<string, ITvContainer>();
            ViewportFactory = viewportFactory;
            DynamicViewportFactory = dynamicViewportFactory;
            engine.UI.ComponentRemoved += Tree_OnComponentRemoved;
        }

        private void Tree_OnComponentRemoved(object sender, TreeUpdatedEventArgs e)
        {
            var name = e.ComponentMetadata.Component.Name;
            if (_containers.ContainsKey(name))
            {
                _containers.Remove(name);
            }
        }


        public void Add(ITvContainer container)
        {
            _containers.Add(container.Name, container);
        }


        public T? Get<T>(string name) where T : class, ITvContainer =>
            _containers.TryGetValue(name, out var container) ? container as T : default;

    }
}
