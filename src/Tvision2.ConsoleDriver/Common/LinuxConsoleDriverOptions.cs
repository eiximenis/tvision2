using System;
using Tvision2.ConsoleDriver.Common;

namespace Tvision2.ConsoleDriver.Common
{
    public class LinuxConsoleDriverOptions : ConsoleDriverOptions, ILinuxConsoleDriverOptions, 
        ILinuxAnsiDriverOptions, INCursesDriverOptions
    {
        public bool UseNCurses { get; private set; }
        
        public TrueColorOptions  TrueColorOptions { get; }
        
        public PaletteOptions PaletteOptions { get; }

        public LinuxConsoleDriverOptions()
        {
            UseNCurses = false;
            TrueColorOptions = new TrueColorOptions();
            PaletteOptions = new PaletteOptions();
        }

        INCursesDriverOptions ILinuxConsoleDriverOptions.UseNCurses()
        {
            UseNCurses = true;
            return this;
        }
        

        ILinuxAnsiDriverOptions ILinuxConsoleDriverOptions.UseAnsi()
        {
            UseNCurses = false;
            return this;
        }
        
        

        public ILinuxAnsiDriverOptions EnableTrueColor(Action<ITrueColorOptions> truecolorOptionsAction = null)
        {
            truecolorOptionsAction?.Invoke(TrueColorOptions);
            PaletteOptions.TrueColorEnabled = TrueColorOptions.Provider != TrueColorProvider.None;
            return this;
        }

        INCursesDriverOptions INCursesDriverOptions.WithPalette(Action<IPaletteOptions> paletteOptionsAction = null)
        {
            paletteOptionsAction?.Invoke(PaletteOptions);
            return this;
        }
        
        ILinuxAnsiDriverOptions ILinuxAnsiDriverOptions.WithPalette(Action<IPaletteOptions> paletteOptionsAction = null)
        {
            paletteOptionsAction?.Invoke(PaletteOptions);
            return this;
        }
    }
}