using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Render;

namespace Tvision2.Viewports
{
    class DynamicViewportFactory : IDynamicViewportFactory
    {
        public readonly IViewportFactory _factory;
        public DynamicViewportFactory(IViewportFactory factory)
        {
            _factory = factory;
        }

        public IViewport Create(Func<IViewportFactory, IViewport> creator)
        {
            return new DynamicViewport(_factory, creator);
        }
    }
}
