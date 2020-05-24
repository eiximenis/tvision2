using System.Collections.Generic;

namespace Tvision2.ConsoleDriver.Ansi.Input
{
    public interface IInputSequences
    {
        IEnumerable<InputSequence> GetSequences();
    }
}