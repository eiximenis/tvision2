using System;

namespace Tvision2.ConsoleDriver.Ansi.Input
{
    public class InputSequence
    {
        public string Sequence { get; }
        public ConsoleKey Key { get; }

        public InputSequence(string seq, ConsoleKey key)
        {
            Sequence = seq;
            Key = key;
        }
    }
}