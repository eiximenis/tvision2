using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Render;
using Tvision2.Engine.Console;

namespace Tvision2.Viewports
{
    class DynamicViewportFactory : IDynamicViewportFactory
    {
        private readonly ViewportFactory _vpFactory;

        public DynamicViewportFactory(IConsoleDriver console)
        {
            _vpFactory = new ViewportFactory(console);
        }

        public IViewport Create(Func<IViewportFactory, IViewport> creator)
        {
            return new DynamicViewport(_vpFactory, creator);
        }

        public IViewport FullViewport()
        {
            return Create(_vpFactory => _vpFactory.FullViewport());
        }

        public IViewport BottomViewport(int vprows = 1)
        {
            return Create(_vpFactory => _vpFactory.BottomViewport(vprows));
        }
    }
}
