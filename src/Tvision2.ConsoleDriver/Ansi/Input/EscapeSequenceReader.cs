using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using Tvision2.Core.Colors;

namespace Tvision2.ConsoleDriver.Ansi.Input
{

    public class SequenceNode
    {
        public char Token { get; }
        public bool EndNode { get; }
        
        
        public SortedList<char, SequenceNode> Childs { get;}

        public  SequenceNode(char token,  bool leaf)
        {
            
            Token = token;
            EndNode = leaf;
            if (!EndNode)
            {
                Childs = new SortedList<char, SequenceNode>();
            }
        }
        
    }
    
    
    public class EscapeSequenceReader
    {
        private SortedList<char, SequenceNode> _sequences;
        private int _idx;
        private int[] _currentSequence;

        public EscapeSequenceReader()
        {
            _sequences = new SortedList<char, SequenceNode>();
            _idx = 0;
            _currentSequence = new int[10];
        }


        public void AddSequences(IEnumerable<string> sequences)
        {
            foreach (var sequence in sequences)
            {
                AddSequence(sequence);
            }
        }

        private void AddSequence(string sequence)
        {
            var list = _sequences;
            var idx = 0;
            foreach (var token in sequence)
            {
                var lastToken = idx == sequence.Length - 1;
                if (!list.ContainsKey(token))
                {
                    var node = new SequenceNode(token, lastToken);
                    list.Add(token, node);
                    list = node.Childs;
                }
                else
                {
                    list = list[token].Childs;
                }

                idx++;
            }
            
            
        }

        public int CheckSequence()
        {
            return 0;
        }

        public void Push(in int nextkey)
        {
            _currentSequence[_idx] = nextkey;
            _idx++;
        }

        public void Start()
        {
            Array.Clear(_currentSequence, 0, _currentSequence.Length);
            _idx = 0;
        }
    }
}