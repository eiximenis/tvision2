﻿using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Engine.Console;

namespace Tvision2.Layouts
{
    class LayoutManager : ILayoutManager
    {
        private Dictionary<string, ITvContainer> _containers;
        public IViewportFactory ViewportFactory { get; }
        public LayoutManager(IViewportFactory viewportFactory)
        {
            _containers = new Dictionary<string, ITvContainer>();
            ViewportFactory = viewportFactory;
        }
        public void Add(ITvContainer container)
        {
            _containers.Add(container.Name, container);
        }

        public void Remove(ITvContainer container)
        {
            if (_containers.ContainsKey(container.Name))
            {
                _containers.Remove(container.Name);
            }
        }

        public T Get<T>(string name) where T : class, ITvContainer =>
            _containers.TryGetValue(name, out ITvContainer container) ? container as T : default(T);

    }
}
