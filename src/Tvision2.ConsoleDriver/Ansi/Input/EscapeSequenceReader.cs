using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Tvision2.Core.Colors;

namespace Tvision2.ConsoleDriver.Ansi.Input
{

    public class SequenceNode
    {
        public char Token { get; }
        public bool EndNode { get; }
        
        public string FullSequence { get; }
        
        
        public ConsoleKeyInfo KeyInfo { get; }
        
        
        public SortedList<char, SequenceNode> Childs { get;}

        public  SequenceNode(char token,  string fullSequence, bool leaf, ConsoleKeyInfo keyInfo)
        {
            
            Token = token;
            EndNode = leaf;
            FullSequence = fullSequence;
            KeyInfo = keyInfo;
            if (!EndNode)
            {
                Childs = new SortedList<char, SequenceNode>();
            }
        }
        
    }
    
    
    public class EscapeSequenceReader
    {
        private SortedList<char, SequenceNode> _sequences;
        private int _count;
        private int _currentIdx;
        private int[] _currentSequence;

        public EscapeSequenceReader()
        {
            _sequences = new SortedList<char, SequenceNode>();
            _count = 0;
            _currentSequence = new int[10];
        }


        public void AddSequences(IEnumerable<InputSequence> sequences)
        {
            foreach (var sequence in sequences)
            {
                AddSequence(sequence);
            }
        }

        private void AddSequence(InputSequence sequence)
        {
            var list = _sequences;
            var idx = 0;
            var secStr = sequence.Sequence;
            foreach (var token in secStr)
            {
                var lastToken = idx == secStr.Length - 1;
                if (!list.ContainsKey(token))
                {
                    var key = lastToken ? sequence.KeyInfo : new ConsoleKeyInfo();
                    var node = new SequenceNode(token, secStr.Substring(0, idx+1), lastToken, key);
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


        public IEnumerable<SequenceNode> CheckSequences()
        {
            while (_currentIdx < _count)
            {
                var seqNode = CheckSequence();
                yield return seqNode;
                if (seqNode == null)
                {
                    while (_currentSequence[_currentIdx] != 27 && _currentIdx < _count)
                    {
                        _currentIdx++;
                    }
                }
            }
        }

        public SequenceNode?  CheckSequence()
        {
            var list = _sequences;
            SequenceNode? node = null;
            while (_currentIdx < _count)
            {
                if (!list.TryGetValue((char) _currentSequence[_currentIdx], out node))
                {
                    return null;
                }
                list = node.Childs;
                _currentIdx++;
                if (node.EndNode) break;
            }

            return node;
        }

        public void Push(in int nextkey)
        {
            _currentSequence[_count] = nextkey;
            _count++;
        }

        public void PushFullSequence(ReadOnlySpan<char> sequence)
        {
            Start();
            for (var idx = 0; idx < sequence.Length; idx++)
            {
                _currentSequence[idx] = sequence[idx];
            }
            _count = sequence.Length;
        }

        public void Start()
        {
            Array.Clear(_currentSequence, 0, _currentSequence.Length);
            _currentIdx = 0;
            _count = 0;
        }
    }
}