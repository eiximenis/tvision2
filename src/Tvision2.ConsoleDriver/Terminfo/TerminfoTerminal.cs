using System.Collections.Generic;
using Microsoft.Extensions.Hosting;

namespace Tvision2.ConsoleDriver.Terminfo
{
    public class TerminfoTerminal
    {
        private string _cup;
        private string _ichl;
        private string _max_colors;

        public TerminfoTerminal()
        {

        }
        
        
        public void Load()
        {
            _cup =  TerminfoBindings.tigetstr("cup");
            _ichl = TerminfoBindings.tigetstr("ichl");
            _max_colors = TerminfoBindings.tigetstr("colors");
        }

        public string Cup(int row, int col)
        {
            return TerminfoBindings.tparm(_cup, row, col);
        } 

        public string Ichl(char c) => TerminfoBindings.tparm(_ichl, c);
        
       

    }
}