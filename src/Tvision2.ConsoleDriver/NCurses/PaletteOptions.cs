using System;
using System.Drawing;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using Tvision2.ConsoleDriver.ColorDefinitions;
using Tvision2.Core.Colors;

namespace Tvision2.ConsoleDriver.Common
{
    public class PaletteOptions : IPaletteOptions, IPaletteOptionsWithPaletteInitialized
    {
        
        public UpdateTerminalEntries UpdateTerminalEntries { get; private set; }
        public  string PaletteToLoad { get; private set; }
        
        public IRgbColortranslator ColorTranslator { get; private set; }

        public IPaletteDefinitionParser PaletteParser { get; private set; }

        public bool ForceBasicPalette { get; private set; }
        
        public bool TrueColorEnabled { get; set; }
        
        public PaletteOptions()
        {
            TrueColorEnabled = false;
            UpdateTerminalEntries = UpdateTerminalEntries.None;
            PaletteToLoad = null;
            PaletteParser = DefaultPaletteDefinitionParser.Instance;
            ColorTranslator = null;
            ForceBasicPalette = false;
        }
        
        void IPaletteOptions.UseBasicColorMode()
        {
            ForceBasicPalette = true;
        }


        public bool SupportRgbColors => ColorTranslator != null;
        
        
        IPaletteOptionsWithPaletteInitialized IPaletteOptions.LoadFromTerminalName(string name = null)
        {
            name = name ?? Environment.GetEnvironmentVariable("TERM");
            PaletteToLoad = name;
            return this;
        }
        

        IPaletteOptionsWithPaletteInitialized IPaletteOptions.LoadFromDefinition()
        {
            return this;
        }
        
        void IPaletteOptions.TranslateRgbColorsWith(IRgbColortranslator translator)
        {
            ColorTranslator = translator;
        }

        void IPaletteOptions.TranslateRgbColorsWith(
            Func<TvColor, IPalette, int> translatorFunc)
        {
            ColorTranslator = new DelegateRgbColorTranslator(translatorFunc);
        }
        
        IPaletteOptionsWithPaletteInitialized IPaletteOptionsWithPaletteInitialized.UpdateTerminal(UpdateTerminalEntries entriesToUpdate = UpdateTerminalEntries.AllButAnsi4bit)
        {
            UpdateTerminalEntries = entriesToUpdate;
            return this;
        }

        IPaletteOptionsWithPaletteInitialized IPaletteOptionsWithPaletteInitialized.ParseWith(IPaletteDefinitionParser parser)
        {
            PaletteParser = parser;
            return this;
        }

        IPaletteOptionsWithPaletteInitialized IPaletteOptionsWithPaletteInitialized.ParseWith(
            Func<string, (string name, int idx, TvColor rgbColor)> parserFunc)
        {
            PaletteParser = new DelegatePaletteParser(parserFunc);
            return this;
        }
    }

    public enum UpdateTerminalEntries
    {
        None,
        AllButAnsi3bit,
        AllButAnsi4bit,
        All
    }
}