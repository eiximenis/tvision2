using System;
using Tvision2.Core.Render;

namespace Tvision2.Core.Engine
{
    public class ViewportUpdatedEventArgs
    {
        public IViewport Previous { get; }
        public IViewport Current { get; }
        public Guid Id { get; }
        public string ComponentName { get; }

        public ViewportUpdatedEventArgs(Guid guid, IViewport previous, IViewport current, string componentName)
        {
            Id = guid;
            Previous = previous ?? Viewport.NullViewport;
            Current = current;
            ComponentName = componentName;
        }
    }
}