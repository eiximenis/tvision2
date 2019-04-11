using System;
using Tvision2.ConsoleDriver.Terminfo;

namespace Tvision2.ConsoleDriver.Common
{
    public class DirectAccessOptions : IDirectAccessOptions
    {
        public TrueColorOptions TrueColorOptions { get;  }
        public bool TrueColorEnabled { get; private set; }

        public DirectAccessOptions()
        {
            TrueColorEnabled = false;
            TrueColorOptions = new TrueColorOptions();
        }
        
        ITrueColorOptions IDirectAccessOptions.UseTrueColor()
        {
            TrueColorEnabled = true;
            return TrueColorOptions;
        }
        
        
    }
}