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
        
        public bool UpdateTerminal { get; private set; }
        public  string PaletteToLoad { get; private set; }
        
        public IRgbColortranslator ColorTranslator { get; private set; }

        public IPaletteDefinitionParser PaletteParser { get; private set; }
        
        public PaletteOptions()
        {
            UpdateTerminal = false;
            PaletteToLoad = null;
            PaletteParser = new DefaultPaletteDefinitionParser();
            ColorTranslator = null;
        }

        public bool SupportRgbColors => ColorTranslator != null;
        
        
        IPaletteOptionsWithPaletteInitialized IPaletteOptions.InitFromTerminalName(string name = null)
        {
            name = name ?? Environment.GetEnvironmentVariable("TERM");
            PaletteToLoad = name;
            return this;
        }
        

        IPaletteOptionsWithPaletteInitialized IPaletteOptions.InitFromDefinition()
        {
            return this;
        }
        
        IPaletteOptionsWithPaletteInitialized IPaletteOptionsWithPaletteInitialized.UpdateTerminal(bool update = true)
        {
            UpdateTerminal = update;
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

        IPaletteOptionsWithPaletteInitialized IPaletteOptionsWithPaletteInitialized.TranslateRgbColorsWith(IRgbColortranslator translator)
        {
            ColorTranslator = translator;
            return this;
        }

        IPaletteOptionsWithPaletteInitialized IPaletteOptionsWithPaletteInitialized.TranslateRgbColorsWith(
            Func<TvColor, IPalette, int> translatorFunc)
        {
            ColorTranslator = new DelegateRgbColorTranslator(translatorFunc);
            return this;
        }
    }
}