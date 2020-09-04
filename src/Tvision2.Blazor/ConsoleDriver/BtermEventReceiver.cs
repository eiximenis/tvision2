using Microsoft.JSInterop;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Diagnostics;
using Tvision2.ConsoleDriver.Ansi;
using Tvision2.ConsoleDriver.Ansi.Input;
using Tvision2.Events;

namespace Tvision2.ConsoleDriver.Blazor
{
    public class BtermEventReceiver
    {
        private readonly XtermSequences _inputSequences;
        private readonly EscapeSequenceReader _secuenceReader;
        private char ESC = '\u001b';
        private TvConsoleEvents _events;
        private readonly object _syncLock;

        public BtermEventReceiver()
        {
            _inputSequences = new XtermSequences();
            _secuenceReader = new EscapeSequenceReader();
            _secuenceReader.AddSequences(_inputSequences.GetSequences());
            _syncLock = new object();
            _events = null;
        }

        public ITvConsoleEvents CurrentEvents
        {
            get
            {
                return _events?.Clone() ?? TvConsoleEvents.Empty;
            }
        }

        [JSInvokable]
        public void OnResize(int cols, int rows)
        {

            _events = _events ?? new TvConsoleEvents();
            _events.SetWindowEvent(new TvWindowEvent(cols, rows));
        }

        [JSInvokable]
        public void OnKeyDown(string key)
        {

            _events = _events ?? new TvConsoleEvents();

            Debug.WriteLine("+++++ KEY " + key);
            if (key[0] == ESC)
            {
                _secuenceReader.PushFullSequence(key.AsSpan(1));
                var node = _secuenceReader.CheckSequence();
                if (node != null)
                {
                    _events.Add(new AnsiConsoleKeyboardEvent(node.KeyInfo));
                }
                else
                {
                    Debug.WriteLine("++++++ SEQ IGNORED: " + key);
                }
            }
            else
            {
                var data = (int)key[0];
                AnsiConsoleKeyboardEvent evt = null;
                switch (data)
                {
                    case 9:
                        evt = new AnsiConsoleKeyboardEvent(new ConsoleKeyInfo('\t', ConsoleKey.Tab, false, false, false));
                        break;
                    case 13:
                        evt = new AnsiConsoleKeyboardEvent(new ConsoleKeyInfo('\r', ConsoleKey.Enter, false, false, false));
                        break;
                    case 127:
                        evt = new AnsiConsoleKeyboardEvent(new ConsoleKeyInfo((char)data, ConsoleKey.Backspace, false, false, false));
                        break;
                    default:
                        if (data < 26)        // 1 is ^A ... 26 is ^Z. Note that 9 is also ^I and ^M is 13 both already handled before
                        {
                            evt = new AnsiConsoleKeyboardEvent(new ConsoleKeyInfo((char)(data - 1 + 'A'), ConsoleKey.A + data - 1, false, false, true));
                        }
                        else
                        {
                            evt = new AnsiConsoleKeyboardEvent(new ConsoleKeyInfo((char)data, (ConsoleKey)data, false, false, false));
                        }
                        break;
                }

                _events.Add(evt);
            }
        }

    }
}
