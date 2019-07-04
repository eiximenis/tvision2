using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Tvision2.ConsoleDriver.Common;
using Tvision2.Core;
using Tvision2.Core.Colors;
using Unix.Terminal;

namespace Tvision2.ConsoleDriver.Common
{
    class NcursesPalette : BasePalette, IPalette
    {

        private readonly PaletteOptions _options;
        
        public bool IsFreezed { get; private set; }

        public ColorMode ColorMode { get; private set; }
        
        public NcursesPalette(PaletteOptions options)
        {
            IsFreezed = true;
            MaxColors = 0;
            ColorMode = ColorMode.Palettized;
            _options = options;
        }

        public bool RedefineColor(int idx, TvColor newColor)
        {
            if (IsFreezed || !newColor.IsRgb)
            {
                return false;
            }

            var (red, green, blue) = newColor.Rgb;
            Curses.InitColor((short) idx, (short) red, (short) green, (short) blue);
            SetColorAt(idx, newColor);
            return true;
        }

        internal void Init()
        {
            if (Curses.HasColors)
            {
                Curses.StartColor();
                IsFreezed = !Curses.CanChangeColor();
                MaxColors = Curses.Colors;
                InitSize(MaxColors);
                LoadPalette();
            }
            else
            {
                ColorMode = ColorMode.NoColors;
                IsFreezed = true;
                MaxColors = 2;
                InitSize(1);
                SetColorAt(0, TvColor.Black);
                SetColorAt(0, TvColor.White);
            }
        }

        private void LoadPalette()
        {
            
            if (!string.IsNullOrEmpty(_options.PaletteToLoad))
            {
                var assembly = Assembly.GetExecutingAssembly();
                var stream =
                    assembly.GetManifestResourceStream(
                        $"Tvision2.ConsoleDriver.ColorDefinitions.{_options.PaletteToLoad}.txt");
                using (var reader = new StreamReader(stream, Encoding.UTF8))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var parsedLine = _options.PaletteParser.ParseLine(line);
                        SetColorAt(parsedLine.idx, parsedLine.rgbColor, parsedLine.name);
                    }
                } 

            }
            
        }
        
        
    }
}