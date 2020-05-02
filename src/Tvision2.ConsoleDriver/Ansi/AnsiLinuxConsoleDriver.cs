using System;
using System.Text;
using Tvision2.ConsoleDriver.Common;
using Tvision2.Core.Colors;
using Tvision2.Core.Render;
using Tvision2.Engine.Console;
using Tvision2.Events;

namespace Tvision2.ConsoleDriver.Ansi
{
    public class AnsiLinuxConsoleDriver : IConsoleDriver
    {

        private readonly LinuxConsoleDriverOptions _options;
        private readonly AnsiColorManager _colorManager;
        
        public TvBounds ConsoleBounds { get; private set; }

        public IColorManager ColorManager
        {
            get => _colorManager;
        }


        public AnsiLinuxConsoleDriver(LinuxConsoleDriverOptions options, AnsiColorManager colorManager)
        {
            _options = options;
            _colorManager = colorManager;
        }
        
        
        public void Init()
        {
            ConsoleBounds = TvBounds.FromRowsAndCols(Console.WindowHeight, Console.WindowWidth);
            
            Console.Out.Write(AnsiEscapeSequences.SMCUP);
            Console.Out.Flush();
            Console.Out.Write(AnsiEscapeSequences.CLEAR);
            Console.Out.Flush();
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
            return TvConsoleEvents.Empty;
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