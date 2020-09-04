using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.ConsoleDriver;
using Tvision2.ConsoleDriver.Common;

namespace Tvision2.ConsoleDriver.Blazor
{
    public class BlazorConsoleDriverOptions : ConsoleDriverOptions, IBlazorConsoleDriverOptions
    {
        public TrueColorOptions TrueColorOptions { get; }
        public PaletteOptions PaletteOptions { get; }

        public BlazorConsoleDriverOptions()
        {
            TrueColorOptions = new TrueColorOptions();
            PaletteOptions = new PaletteOptions() { TrueColorEnabled = true };
        }

        IBlazorConsoleDriverOptions IBlazorConsoleDriverOptions.WithPalette(Action<IPaletteOptions> paletteOptionsAction = null)
        {
            paletteOptionsAction?.Invoke(PaletteOptions);
            return this;
        }

    }

    public interface IBlazorConsoleDriverOptions : IConsoleDriverOptions
    {
        IBlazorConsoleDriverOptions WithPalette(Action<IPaletteOptions> paletteOptionsAction = null);
    }
}
