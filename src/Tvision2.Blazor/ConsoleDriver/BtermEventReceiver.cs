using Microsoft.JSInterop;
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
        private bool _needToClearEvents;

        public BtermEventReceiver()
        {
            _inputSequences = new XtermSequences();
            _secuenceReader = new EscapeSequenceReader();
            _secuenceReader.AddSequences(_inputSequences.GetSequences());
            _events = null;
            _needToClearEvents = false;
        }

        public ITvConsoleEvents CurrentEvents
        {
            get
            {
                if (_needToClearEvents)
                {
                    _events = null;
                    _needToClearEvents = false;
                }
                return _events ?? TvConsoleEvents.Empty;
            }
        }

        [JSInvokable]
        public void OnKeyDown(string key)
        {
            if (_needToClearEvents || _events == null)
            {
                _events = new TvConsoleEvents();
                _needToClearEvents = false;
            }

            Debug.WriteLine("+++++ KEY " + key);
            if (key[0] == ESC)
            {
                _secuenceReader.PushFullSequence(key.AsSpan(1));
                var node = _secuenceReader.CheckSequence();
                if (node != null)
                {
                    _events.Add(new AnsiConsoleKeyboardEvent(node.KeyInfo));
                }
            }
            else
            {
                var data = (int)key[0];
                switch (data)
                {
                    case 9:
                        _events.Add(new AnsiConsoleKeyboardEvent(new ConsoleKeyInfo('\t', ConsoleKey.Tab, false, false, false)));
                        break;
                    case 13:
                        _events.Add(new AnsiConsoleKeyboardEvent(new ConsoleKeyInfo('\r', ConsoleKey.Enter, false, false, false)));
                        break;
                    case 127:
                        _events.Add(new AnsiConsoleKeyboardEvent(new ConsoleKeyInfo((char)data, ConsoleKey.Backspace, false, false, false)));
                        break;
                    default:
                        if (data < 26)        // 1 is ^A ... 26 is ^Z. Note that 9 is also ^I and ^M is 13 both already handled before
                        {
                            _events.Add(new AnsiConsoleKeyboardEvent(new ConsoleKeyInfo((char)(data - 1 + 'A'), ConsoleKey.A + data - 1, false, false, true)));
                        }
                        else
                        {
                            _events.Add(new AnsiConsoleKeyboardEvent(new ConsoleKeyInfo((char)data, (ConsoleKey)data, false, false, false)));
                        }
                        break;
                }
            }
        }

        public void DeleteAllEventsOnNextCycle()
        {
            _needToClearEvents = _events != null;
        }
    }
}
