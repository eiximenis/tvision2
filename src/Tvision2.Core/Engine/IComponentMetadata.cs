using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.ConsoleDriver;

namespace Tvision2.Core.Engine
{
    public interface IComponentMetadata
    {
        IConsoleDriver Console { get; }
    }
}
