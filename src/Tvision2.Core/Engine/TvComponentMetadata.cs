using System;
using Tvision2.Core.Components;
using Tvision2.Engine.Console;

namespace Tvision2.Core.Engine
{
    internal class TvComponentMetadata : IComponentMetadata
    {
        public TvComponent Component { get; }

        public IConsoleDriver Console { get; internal set; }

        public event EventHandler ViewportChanged;

        public TvComponentMetadata(TvComponent component)
        {
            Component = component;
        }

        public void OnViewportChanged()
        {
            var handler = ViewportChanged;
            handler?.Invoke(this, EventArgs.Empty);
        }
    }

}
