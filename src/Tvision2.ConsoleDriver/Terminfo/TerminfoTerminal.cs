using System.Collections.Generic;
using Microsoft.Extensions.Hosting;

namespace Tvision2.ConsoleDriver.Terminfo
{
    public class TerminfoTerminal
    {


        private string _cup;
        private string _ichl;
        
        
        public void Load()
        {
            _cup =  TerminfoBindings.tigetstr("cup");
            _ichl = TerminfoBindings.tigetstr("ichl");
        }

        public string Cup(int x, int y)
        {
            //var cup =  TerminfoBindings.tigetstr("cup");
            return TerminfoBindings.tparm(_cup, x, y);
        } 

        public string Ichl(char c) => TerminfoBindings.tparm(_ichl, c);




    }
}