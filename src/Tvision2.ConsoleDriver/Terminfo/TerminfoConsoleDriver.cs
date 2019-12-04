using System;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Extensions.FileSystemGlobbing.Internal.PathSegments;
using Tvision2.ConsoleDriver.Terminfo;
using Tvision2.Core.Colors;
using Tvision2.Core.Render;
using Tvision2.Engine.Console;
using Tvision2.Events;
using Unix.Terminal;

namespace Tvision2.ConsoleDriver
{
    public class TerminfoConsoleDriver : IConsoleDriver
    {


        private readonly TerminfoTerminal _terminal;
        private readonly ITerminfoColorManager _colorManager;
        public TvBounds ConsoleBounds { get; private set; }

        public IColorManager ColorManager => _colorManager;

        public TerminfoConsoleDriver(ITerminfoColorManager colorManager)
        {
            _terminal = new  TerminfoTerminal();
            _colorManager = colorManager;
        }
        
        
        public void Init()
        {
            ConsoleBounds = TvBounds.FromRowsAndCols(Console.WindowHeight, Console.WindowWidth);
            var terminalName = System.Environment.GetEnvironmentVariable("TERM");
            var ret = TerminfoBindings.setupterm(terminalName, 1, IntPtr.Zero);
            
            ret = TerminfoBindings.tgetent(IntPtr.Zero, terminalName);
            if (ret < 0)
            {
                throw new InvalidOperationException($"Terminfo database can't be opened. ** FATAL **");
            }

            if (ret == 0)
            {
                throw new InvalidOperationException($"Terminal '${terminalName}' definition not found in Terminfo db. Is TERM value set to the right value?");
            }

            Curses.initscr();
            Curses.raw();;
            var smcup = TerminfoBindings.tigetstr("smcup");

            TerminfoBindings.putp(smcup);
           
            _terminal.Load();
            _colorManager.Init();
        }
        
        public void WriteCharacterAt(int x, int y, char character, CharacterAttribute attribute)
        {   
            TerminfoBindings.putp(_terminal.Cup(y, x));
            _colorManager.SetAttributes(attribute);
            TerminfoBindings.putp(character.ToString());
        }

        public void WriteCharactersAt(int x, int y, int count, char character, CharacterAttribute attribute)
        {
            // TODO: Temporal hack for compilation. Needs to be refactored to make a single native call
            for (var rep = 0; rep < count; rep++)
            {
                WriteCharacterAt(x + rep, y, character, attribute);
            }
        }


        public void SetCursorAt(int x, int y)
        {
            //throw new NotImplementedException();
        }

        public ITvConsoleEvents ReadEvents()
        {
            //throw new NotImplementedException();
            return TvConsoleEvents.Empty;
        }


        public (int rows, int cols) GetConsoleWindowSize()
        {
            return (24,80);
        }

        public void SetCursorVisibility(bool isVisible)
        {
        }

        public void ProcessWindowEvent(TvWindowEvent windowEvent)
        {
            ConsoleBounds = TvBounds.FromRowsAndCols(Console.WindowHeight, Console.WindowWidth);
            windowEvent.Update(ConsoleBounds.Cols, ConsoleBounds.Rows);
        }
    }
}