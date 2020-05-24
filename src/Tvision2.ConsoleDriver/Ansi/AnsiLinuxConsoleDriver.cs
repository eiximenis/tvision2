using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Tvision2.ConsoleDriver.Ansi.Input;
using Tvision2.ConsoleDriver.Ansi.Interop;
using Tvision2.ConsoleDriver.Common;
using Tvision2.Core.Colors;
using Tvision2.Core.Render;
using Tvision2.Engine.Console;
using Tvision2.Events;

using static Tvision2.ConsoleDriver.Ansi.Interop.Types;

namespace Tvision2.ConsoleDriver.Ansi
{
    public class AnsiLinuxConsoleDriver : IConsoleDriver
    {

        private readonly LinuxConsoleDriverOptions _options;
        private readonly AnsiColorManager _colorManager;
        private readonly EscapeSequenceReader _secuenceReader;
        private readonly IInputSequences _inputSequences;


        private const int ESC = 27;
        
        public TvBounds ConsoleBounds { get; private set; }

        public IColorManager ColorManager
        {
            get => _colorManager;
        }


        public AnsiLinuxConsoleDriver(LinuxConsoleDriverOptions options, AnsiColorManager colorManager)
        {
            _options = options;
            _colorManager = colorManager;
            _inputSequences = new XtermSequences();
            _secuenceReader = new EscapeSequenceReader();
            _secuenceReader.AddSequences(_inputSequences.GetSequences());
        }

        public void Init()
        {
            ConsoleBounds = TvBounds.FromRowsAndCols(Console.WindowHeight, Console.WindowWidth);
            
            
            Console.Out.Write(AnsiEscapeSequences.SMCUP);
            Console.Out.Flush();
            Console.Out.Write(AnsiEscapeSequences.CLEAR);
            Console.Out.Flush();
            // Set RAW mode
            var termios = new termios();
            var retval = Libc.tcgetattr(1, ref termios);
            if (retval != 0)
            {
                throw new InvalidOperationException("tcgetattr returned error. Ensure app terminal is not redirected");
            }

            Libc.cfmakeraw(ref termios);
            retval = Libc.tcsetattr(1, Constants.TCSANOW, ref termios);

            _colorManager.Init();

        }
        
        
        
        public void WriteCharacterAt(int x, int y, char character, CharacterAttribute attribute)
        {
            WriteCharactersAt(x, y, 1, character, attribute);
        }

        public void WriteCharactersAt(int x, int y, int count, char character, CharacterAttribute attribute)
        {
            var sb = new StringBuilder();
            sb.Append(_colorManager.GetCursorSequence(x, y));
            sb.Append(_colorManager.GetAttributeSequence(attribute));
            sb.Append(character, count);
            Console.Write(sb.ToString());
        }

        public void SetCursorAt(int x, int y)
        {
            Console.Write(_colorManager.GetCursorSequence(x, y));
        }

        public ITvConsoleEvents ReadEvents()
        {

            var data = Libc.read();
            if (data == -1)
            {
                return TvConsoleEvents.Empty;
            }
            
            var events = new TvConsoleEvents();

            if (data == ESC)
            {
                var sequenceStarted = false;
                Debug.WriteLine($"RE --> {data} '{(char)data}'");
                _secuenceReader.Start();
                var nextkey = Libc.read();
                while (nextkey != -1)
                {
                    _secuenceReader.Push(nextkey);
                    sequenceStarted = true;
                    nextkey = Libc.read();
                }

                if (sequenceStarted)
                {
                    var sequences = _secuenceReader.CheckSequences();
                    foreach (var seq in sequences)
                    {
                        if (seq != null) events.Add(new AnsiConsoleKeyboardEvent(seq.KeyInfo));
                        else Debug.WriteLine("Unknown ESC sequence");
                    }
                }
                else
                {
                    events.Add(new AnsiConsoleKeyboardEvent(new ConsoleKeyInfo((char)data, ConsoleKey.Escape, false, false, false)));
                }
            }
            else
            {
                events.Add(new AnsiConsoleKeyboardEvent(new ConsoleKeyInfo((char)data, (ConsoleKey)data, false, false, false)));
            }

            return events;
        }

        
        
        public void SetCursorVisibility(bool isVisible)
        {
           
            if (isVisible)
            {
                Console.Write(AnsiEscapeSequences.DECTCEM_VISIBLE);
            }
            else
            {
                Console.Write(AnsiEscapeSequences.DECTCEM_HIDDEN);
            }
        }

        public void ProcessWindowEvent(TvWindowEvent windowEvent)
        {
        }
    }
}