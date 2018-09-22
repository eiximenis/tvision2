using System;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Extensions.FileSystemGlobbing.Internal.PathSegments;
using Tvision2.ConsoleDriver.Terminfo;
using Tvision2.Engine.Console;
using Tvision2.Events;

namespace Tvision2.ConsoleDriver
{
    public class TerminfoConsoleDriver : IConsoleDriver
    {


        private readonly TerminfoTerminal _terminal;

        public TerminfoConsoleDriver()
        {
            _terminal = new  TerminfoTerminal();
        }
        
        
        public void Init()
        {
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
           

            
            var smcup = TerminfoBindings.tigetstr("smcup");
            
            TerminfoBindings.putp(TerminfoBindings.tparm(smcup));
           
            _terminal.Load();
        }
        
        public void WriteCharacterAt(int x, int y, char character)
        {
            TerminfoBindings.putp(_terminal.Cup(x, y));

            TerminfoBindings.putp(character + "");
        }

        public void WriteCharacterAt(int x, int y, char character, ConsoleColor foreColor, ConsoleColor backColor)
        {
            TerminfoBindings.putp(_terminal.Cup(y, x));

            TerminfoBindings.putp(character + "");
        }

        public void SetCursorAt(int x, int y)
        {
            //throw new NotImplementedException();
        }

        public TvConsoleEvents ReadEvents()
        {
            //throw new NotImplementedException();
            return TvConsoleEvents.Empty;
        }


        public (int rows, int cols) GetConsoleWindowSize()
        {
            return (24,80);
        }
    }
}