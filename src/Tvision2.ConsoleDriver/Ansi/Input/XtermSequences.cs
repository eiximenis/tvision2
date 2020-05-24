using System;
using System.Collections.Generic;
using System.Linq;

namespace Tvision2.ConsoleDriver.Ansi.Input
{
    public sealed class XtermSequences : IInputSequences
    {
        private readonly  List<InputSequence> _sequences;

        public XtermSequences()
        {
            _sequences = new List<InputSequence>( );
            
            _sequences.Add(new InputSequence("OA", KeyInfoFromKey(ConsoleKey.UpArrow)));
            _sequences.Add(new InputSequence("OB", KeyInfoFromKey(ConsoleKey.DownArrow)));
            _sequences.Add(new InputSequence("OD", KeyInfoFromKey(ConsoleKey.LeftArrow)));
            _sequences.Add(new InputSequence("OC", KeyInfoFromKey(ConsoleKey.RightArrow)));
        }


        public IEnumerable<InputSequence> GetSequences() => _sequences;
        
        
        private  ConsoleKeyInfo KeyInfoFromKey(ConsoleKey key) => new ConsoleKeyInfo((char)key, key, false, false, false);
    }
}