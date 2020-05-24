using System;

namespace Tvision2.ConsoleDriver.Ansi.Input
{
    public class InputSequence
    {
        public string Sequence { get; }
        public ConsoleKeyInfo KeyInfo { get; }

        public InputSequence(string seq, ConsoleKeyInfo keyInfo)
        {
            Sequence = seq;
            KeyInfo = keyInfo;
        }
    }
}