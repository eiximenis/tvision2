using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.ConsoleDriver;
using Tvision2.Core.Components;
using Tvision2.Core.Render;

namespace Tvision2.Core.Engine
{
    public class TvComponentMetadata : IComponentMetadata
    {
        public TvComponent Component { get; }

        public IConsoleDriver Console { get; }

        public TvComponentMetadata(TvComponent component, IConsoleDriver consoleDriver)
        {
            Component = component;
            Console = consoleDriver;
        }
    }

}
