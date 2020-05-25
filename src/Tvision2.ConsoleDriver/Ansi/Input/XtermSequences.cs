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
            
            // Alt+c
            _sequences.Add(new InputSequence("c", new ConsoleKeyInfo('\0', ConsoleKey.C, false, true, false)));
            
            // Arrows
            _sequences.Add(new InputSequence("OA", new ConsoleKeyInfo('\0', ConsoleKey.UpArrow, false, false, false)));
            _sequences.Add(new InputSequence("OB", new ConsoleKeyInfo('\0', ConsoleKey.DownArrow, false, false, false)));
            _sequences.Add(new InputSequence("OD", new ConsoleKeyInfo('\0', ConsoleKey.LeftArrow, false, false, false)));
            _sequences.Add(new InputSequence("OC", new ConsoleKeyInfo('\0', ConsoleKey.RightArrow, false, false, false)));

            _sequences.Add(new InputSequence("[1;2A", new ConsoleKeyInfo('\0', ConsoleKey.UpArrow, true, false, false)));
            _sequences.Add(new InputSequence("[1;2B", new ConsoleKeyInfo('\0', ConsoleKey.DownArrow, true, false, false)));
            _sequences.Add(new InputSequence("[1;2D", new ConsoleKeyInfo('\0', ConsoleKey.LeftArrow, true, false, false)));
            _sequences.Add(new InputSequence("[1;2C", new ConsoleKeyInfo('\0', ConsoleKey.RightArrow, true, false, false)));
            
            _sequences.Add(new InputSequence("[1;3A", new ConsoleKeyInfo('\0', ConsoleKey.UpArrow, false, true, false)));
            _sequences.Add(new InputSequence("[1;3B", new ConsoleKeyInfo('\0', ConsoleKey.DownArrow, false, true, false)));
            _sequences.Add(new InputSequence("[1;3D", new ConsoleKeyInfo('\0', ConsoleKey.LeftArrow, false, true, false)));
            _sequences.Add(new InputSequence("[1;3C", new ConsoleKeyInfo('\0', ConsoleKey.RightArrow, false, true, false)));
            
            _sequences.Add(new InputSequence("[1;4A", new ConsoleKeyInfo('\0', ConsoleKey.UpArrow, true, true, false)));
            _sequences.Add(new InputSequence("[1;4B", new ConsoleKeyInfo('\0', ConsoleKey.DownArrow, true, true, false)));
            _sequences.Add(new InputSequence("[1;4D", new ConsoleKeyInfo('\0', ConsoleKey.LeftArrow, true, true, false)));
            _sequences.Add(new InputSequence("[1;4C", new ConsoleKeyInfo('\0', ConsoleKey.RightArrow, true, true, false)));

            _sequences.Add(new InputSequence("[1;5A", new ConsoleKeyInfo('\0', ConsoleKey.UpArrow, false, false, true)));
            _sequences.Add(new InputSequence("[1;5B", new ConsoleKeyInfo('\0', ConsoleKey.DownArrow, false, false, true)));
            _sequences.Add(new InputSequence("[1;5D", new ConsoleKeyInfo('\0', ConsoleKey.LeftArrow, false, false, true)));
            _sequences.Add(new InputSequence("[1;5C", new ConsoleKeyInfo('\0', ConsoleKey.RightArrow, false, false, true)));
            
            _sequences.Add(new InputSequence("[1;6A", new ConsoleKeyInfo('\0', ConsoleKey.UpArrow, true, false, true)));
            _sequences.Add(new InputSequence("[1;6B", new ConsoleKeyInfo('\0', ConsoleKey.DownArrow, true, false, true)));
            _sequences.Add(new InputSequence("[1;6D", new ConsoleKeyInfo('\0', ConsoleKey.LeftArrow, true, false, true)));
            _sequences.Add(new InputSequence("[1;6C", new ConsoleKeyInfo('\0', ConsoleKey.RightArrow, true, false, true)));
            
            // PageUp / PadeDown
            _sequences.Add(new InputSequence("[5~", new ConsoleKeyInfo('\0', ConsoleKey.PageUp, false, false, false)));
            _sequences.Add(new InputSequence("[6~", new ConsoleKeyInfo('\0', ConsoleKey.PageDown, false, false, false)));
            
            _sequences.Add(new InputSequence("[5;2~", new ConsoleKeyInfo('\0', ConsoleKey.PageUp, true, false, false)));
            _sequences.Add(new InputSequence("[6;2~", new ConsoleKeyInfo('\0', ConsoleKey.PageDown, true, false, false)));
            
            _sequences.Add(new InputSequence("[5;3~", new ConsoleKeyInfo('\0', ConsoleKey.PageUp, false, true, false)));
            _sequences.Add(new InputSequence("[6;3~", new ConsoleKeyInfo('\0', ConsoleKey.PageDown, false, true, false)));
            
            _sequences.Add(new InputSequence("[5;4~", new ConsoleKeyInfo('\0', ConsoleKey.PageUp, true, true, false)));
            _sequences.Add(new InputSequence("[6;4~", new ConsoleKeyInfo('\0', ConsoleKey.PageDown, true, true, false)));
            
            _sequences.Add(new InputSequence("[5;5~", new ConsoleKeyInfo('\0', ConsoleKey.PageUp, false, false, true)));
            _sequences.Add(new InputSequence("[6;5~", new ConsoleKeyInfo('\0', ConsoleKey.PageDown, false, false, true)));
            
            _sequences.Add(new InputSequence("[5;6~", new ConsoleKeyInfo('\0', ConsoleKey.PageUp, true, false, true)));
            _sequences.Add(new InputSequence("[6;6~", new ConsoleKeyInfo('\0', ConsoleKey.PageDown, true, false, true)));

            // Ins
            _sequences.Add(new InputSequence("[2~", new ConsoleKeyInfo('\0', ConsoleKey.Insert, false, false, false)));
            _sequences.Add(new InputSequence("[2;3~", new ConsoleKeyInfo('\0', ConsoleKey.Insert, false, true, false)));

            // Del
            _sequences.Add(new InputSequence("[3~", new ConsoleKeyInfo('\0', ConsoleKey.Delete, false, false, false)));
            _sequences.Add(new InputSequence("[3;2~", new ConsoleKeyInfo('\0', ConsoleKey.Delete, true, false, false)));
            _sequences.Add(new InputSequence("[3;3~", new ConsoleKeyInfo('\0', ConsoleKey.Delete, false, true, false)));
            _sequences.Add(new InputSequence("[3;4~", new ConsoleKeyInfo('\0', ConsoleKey.Delete, true, true, false)));
            _sequences.Add(new InputSequence("[3;5~", new ConsoleKeyInfo('\0', ConsoleKey.Delete, false, false, true)));
            _sequences.Add(new InputSequence("[3;6~", new ConsoleKeyInfo('\0', ConsoleKey.Delete, true, false, true)));
            
            // Home / End

            _sequences.Add(new InputSequence("OH", new ConsoleKeyInfo('\0', ConsoleKey.Home, false, false, false)));
            _sequences.Add(new InputSequence("OF", new ConsoleKeyInfo('\0', ConsoleKey.End, false, false, false)));
            
            _sequences.Add(new InputSequence("[1;2H", new ConsoleKeyInfo('\0', ConsoleKey.Home, true, false, false)));
            _sequences.Add(new InputSequence("[1;2F", new ConsoleKeyInfo('\0', ConsoleKey.End, true, false, false)));
            
            _sequences.Add(new InputSequence("[1;3H", new ConsoleKeyInfo('\0', ConsoleKey.Home, false, true, false)));
            _sequences.Add(new InputSequence("[1;3F", new ConsoleKeyInfo('\0', ConsoleKey.End, false, true, false)));

            _sequences.Add(new InputSequence("[1;4H", new ConsoleKeyInfo('\0', ConsoleKey.Home, true, true, false)));
            _sequences.Add(new InputSequence("[1;4F", new ConsoleKeyInfo('\0', ConsoleKey.End, true, true, false)));
            
            _sequences.Add(new InputSequence("[1;5H", new ConsoleKeyInfo('\0', ConsoleKey.Home, false, false, true)));
            _sequences.Add(new InputSequence("[1;5F", new ConsoleKeyInfo('\0', ConsoleKey.End, false, false, true)));
            
            _sequences.Add(new InputSequence("[1;6H", new ConsoleKeyInfo('\0', ConsoleKey.Home, true, false, true)));
            _sequences.Add(new InputSequence("[1;6F", new ConsoleKeyInfo('\0', ConsoleKey.End, true, false, true)));
            

            // Keypad
            _sequences.Add(new InputSequence("Oo", new ConsoleKeyInfo('\\', ConsoleKey.Divide, false, false, false)));
            _sequences.Add(new InputSequence("Oj", new ConsoleKeyInfo('*', ConsoleKey.Multiply, false, false, false)));
            _sequences.Add(new InputSequence("Om", new ConsoleKeyInfo('-', ConsoleKey.Subtract, false, false, false)));
            _sequences.Add(new InputSequence("Ok", new ConsoleKeyInfo('+', ConsoleKey.Add, false, false, false)));
            
            _sequences.Add(new InputSequence("O2o", new ConsoleKeyInfo('\\', ConsoleKey.Divide, true, false, false)));
            _sequences.Add(new InputSequence("O2j", new ConsoleKeyInfo('*', ConsoleKey.Multiply, true, false, false)));

            _sequences.Add(new InputSequence("O3o", new ConsoleKeyInfo('\\', ConsoleKey.Divide, false, true, false)));
            _sequences.Add(new InputSequence("O3j", new ConsoleKeyInfo('*', ConsoleKey.Multiply, false, true, false)));
            _sequences.Add(new InputSequence("O3m", new ConsoleKeyInfo('-', ConsoleKey.Subtract, false, true, false)));
            _sequences.Add(new InputSequence("O3k", new ConsoleKeyInfo('+', ConsoleKey.Add, false, true, false)));
            
            _sequences.Add(new InputSequence("O4o", new ConsoleKeyInfo('\\', ConsoleKey.Divide, true, true, false)));
            _sequences.Add(new InputSequence("O4j", new ConsoleKeyInfo('*', ConsoleKey.Multiply, true, true, false)));

            _sequences.Add(new InputSequence("O5o", new ConsoleKeyInfo('\\', ConsoleKey.Divide, false ,false, true)));
            _sequences.Add(new InputSequence("O5j", new ConsoleKeyInfo('*', ConsoleKey.Multiply, false, false, true)));
            
            _sequences.Add(new InputSequence("O6o", new ConsoleKeyInfo('\\', ConsoleKey.Divide, true, false, true)));
            _sequences.Add(new InputSequence("O6j", new ConsoleKeyInfo('*', ConsoleKey.Multiply, true, false, true)));
            
            // Function Keys
            _sequences.Add(new InputSequence("OP", new ConsoleKeyInfo('\0', ConsoleKey.F1, false, false, false)));
            _sequences.Add(new InputSequence("OQ", new ConsoleKeyInfo('\0', ConsoleKey.F2, false, false, false)));
            _sequences.Add(new InputSequence("OR", new ConsoleKeyInfo('\0', ConsoleKey.F3, false, false, false)));
            _sequences.Add(new InputSequence("OS", new ConsoleKeyInfo('\0', ConsoleKey.F4, false, false, false)));
            _sequences.Add(new InputSequence("OS", new ConsoleKeyInfo('\0', ConsoleKey.F4, false, false, false)));
            _sequences.Add(new InputSequence("[15~ ", new ConsoleKeyInfo('\0', ConsoleKey.F5, false, false, false)));
            _sequences.Add(new InputSequence("[17~ ", new ConsoleKeyInfo('\0', ConsoleKey.F6, false, false, false)));
            _sequences.Add(new InputSequence("[18~ ", new ConsoleKeyInfo('\0', ConsoleKey.F7, false, false, false)));
            _sequences.Add(new InputSequence("[19~ ", new ConsoleKeyInfo('\0', ConsoleKey.F8, false, false, false)));
            _sequences.Add(new InputSequence("[20~ ", new ConsoleKeyInfo('\0', ConsoleKey.F9, false, false, false)));

            _sequences.Add(new InputSequence("[1;2P", new ConsoleKeyInfo('\0', ConsoleKey.F1, true, false, false)));
            _sequences.Add(new InputSequence("[1;2Q", new ConsoleKeyInfo('\0', ConsoleKey.F2, true, false, false)));
            _sequences.Add(new InputSequence("[1;2R", new ConsoleKeyInfo('\0', ConsoleKey.F3, true, false, false)));
            _sequences.Add(new InputSequence("[1;2S", new ConsoleKeyInfo('\0', ConsoleKey.F4, true, false, false)));
            _sequences.Add(new InputSequence("[15;2~", new ConsoleKeyInfo('\0', ConsoleKey.F5, true, false, false)));
            _sequences.Add(new InputSequence("[17;2~", new ConsoleKeyInfo('\0', ConsoleKey.F6, true, false, false)));
            _sequences.Add(new InputSequence("[18;2~", new ConsoleKeyInfo('\0', ConsoleKey.F7, true, false, false)));
            _sequences.Add(new InputSequence("[19;2~", new ConsoleKeyInfo('\0', ConsoleKey.F8, true, false, false)));
            _sequences.Add(new InputSequence("[20;2~", new ConsoleKeyInfo('\0', ConsoleKey.F9, true, false, false)));
            
            _sequences.Add(new InputSequence("[1;3R", new ConsoleKeyInfo('\0', ConsoleKey.F3, false, true, false)));
            _sequences.Add(new InputSequence("[20;3~", new ConsoleKeyInfo('\0', ConsoleKey.F9, false, true, false)));
            _sequences.Add(new InputSequence("[23;3~", new ConsoleKeyInfo('\0', ConsoleKey.F11, false, true, false)));
            _sequences.Add(new InputSequence("[24;3~", new ConsoleKeyInfo('\0', ConsoleKey.F12, false, true, false)));
        }


        public IEnumerable<InputSequence> GetSequences() => _sequences;
        
    }
}