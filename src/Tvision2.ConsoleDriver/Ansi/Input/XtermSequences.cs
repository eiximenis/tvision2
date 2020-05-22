using System;
using System.Collections.Generic;
using System.Linq;

namespace Tvision2.ConsoleDriver.Ansi.Input
{
    public sealed class XtermSequences : IInputSequences
    {
        private readonly  Dictionary<string, InputSequence> _sequences;

        public XtermSequences()
        {
            _sequences = new Dictionary<string, InputSequence>( );
            
            _sequences.Add("OA", new InputSequence("OA", ConsoleKey.UpArrow));
            _sequences.Add("OB", new InputSequence("OB", ConsoleKey.DownArrow));
            _sequences.Add("OD", new InputSequence("OD", ConsoleKey.LeftArrow));
            _sequences.Add("OC", new InputSequence("OC", ConsoleKey.RightArrow));
        }


        public IEnumerable<string> GetSequences() => _sequences.Keys;
    }
}