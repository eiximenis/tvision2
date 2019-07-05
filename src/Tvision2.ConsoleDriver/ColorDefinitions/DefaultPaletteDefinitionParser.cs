using System;
using System.Globalization;
using Tvision2.Core.Colors;

namespace Tvision2.ConsoleDriver.ColorDefinitions
{
    public class DefaultPaletteDefinitionParser : IPaletteDefinitionParser
    {
        private static DefaultPaletteDefinitionParser _instance;

        static DefaultPaletteDefinitionParser() => _instance = new DefaultPaletteDefinitionParser();

        public static IPaletteDefinitionParser Instance => _instance; 
        
        
        public (string name, int idx, TvColor rgbColor) ParseLine(string line)
        {
            line.Trim();
            var tokens = line.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
            if (tokens.Length == 3)
            {
                return Parse3TokensLine(tokens);
            }

            if (tokens.Length == 5)
            {
                return Parse5TokensLine(tokens);
            }

            throw new InvalidOperationException(
                $"Line '{line}' is invalid ({tokens.Length} tokens found and 3 or 5 were expected");
        }

        /*
         * Two formats accepted
         * idx name #rrggbb (where rr gg bb is Hex 00-FF) --> 7 Silver  #c0c0c0 
         * idx name r,g,b (where r,g,b is decimal 0-255 or hex 0x00-0xFF) --> 7 Silver 0xc0 0Xc0 192 
         */
        private (string name, int idx, TvColor rgbColor) Parse3TokensLine(string[] tokens)
        {
            var idx = int.Parse(tokens[0]);
            var name = tokens[1];
            var color = tokens[2];

            byte red, green, blue;
            
            if (color[0] == '#')
            {
                red = byte.Parse(color.Substring(1, 2), NumberStyles.HexNumber);
                green = byte.Parse(color.Substring(3, 2), NumberStyles.HexNumber);
                blue = byte.Parse(color.Substring(5,2), NumberStyles.HexNumber);
            }
            else
            {
                var rgb = color.Split(',');
                red = ParseDecimalOrHexToken(rgb[0]);
                green = ParseDecimalOrHexToken(rgb[1]);
                blue = ParseDecimalOrHexToken(rgb[2]);
            }

            return (name, idx, TvColor.FromRGB(red, green, blue));
        }

        /*
            * One format accepted
            * idx name r g b (where r g b is decimal 0-255 or Hex (0x0-0xFF)
        */
        private (string name, int idx, TvColor rgbColor) Parse5TokensLine(string[] tokens)
        {
            var idx = int.Parse(tokens[0]);
            var name = tokens[1];
            var red = ParseDecimalOrHexToken(tokens[2]);
            var green = ParseDecimalOrHexToken(tokens[3]);
            var blue = ParseDecimalOrHexToken(tokens[4]);
            return (name, idx, TvColor.FromRGB(red, green, blue));
        }

        private byte ParseDecimalOrHexToken(string token)
        {
            if (token.StartsWith("0x") || token.StartsWith("0X"))
            {
                return byte.Parse(token.Substring(2), NumberStyles.HexNumber);
            }

            return byte.Parse(token);
        }
    }
}