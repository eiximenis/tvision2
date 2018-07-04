using System;
using Tvision2.Core.Components;
using Tvision2.Engine.Console;

namespace Tvision2.Core.Engine
{
    internal class TvComponentMetadata : IComponentMetadata
    {
        public TvComponent Component { get; }

        public IConsoleDriver Console { get; }

        public event EventHandler ViewportChanged;

        public TvComponentMetadata(TvComponent component, IConsoleDriver consoleDriver)
        {
            Component = component;
            Component.SetMetadata(this);
            Console = consoleDriver;
        }

        public void OnViewportChanged()
        {
            var handler = ViewportChanged;
            handler?.Invoke(this, EventArgs.Empty);
        }
    }

}
