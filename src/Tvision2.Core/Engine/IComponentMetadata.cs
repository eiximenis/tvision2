using System;
using Tvision2.Core.Components;
using Tvision2.Engine.Console;

namespace Tvision2.Core.Engine
{
    public interface IComponentMetadata
    {
        TvComponent Component { get; }
        IConsoleDriver Console { get; }
        event EventHandler ViewportChanged;
    }
}
