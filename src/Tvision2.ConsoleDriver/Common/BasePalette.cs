using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Tvision2.Core;
using Tvision2.Core.Colors;

namespace Tvision2.ConsoleDriver.Common
{
    abstract  class BasePalette
    {
        private TvColor?[] _colors;
        private readonly Dictionary<string, int> _colorIndexes;
        private int _colorsAdded;

        public int MaxColors { get; protected set; }
        public bool IsFull => _colorsAdded == MaxColors;

        public BasePalette()
        {
            _colorIndexes = new Dictionary<string, int>();
            _colorsAdded = 0;
        }

        protected void InitSize(int size)
        {
            MaxColors = size;
            _colors = new TvColor?[size];
        }


        protected   void AddColor(int idx, TvColor color, string name)
        {
            if (_colors[idx].HasValue)
            {
                throw new ArgumentException($"Index {idx} was already added with color {_colors[idx].ToString()}");
            }
            _colors[idx] = color;
            
            if (name != null)
            {
                _colorIndexes.Add(name, idx);
            }

            _colorsAdded++;
            
        }

        public int AddColor(TvColor color, string name = null)
        {
            if (IsFull)
            {
                return -1;
            }

            var idx = FirstEmptyIndex();
            AddColor(idx, color, name);
            OnColorAdded(color, idx);
            return idx;
        } 
        
        private  int FirstEmptyIndex()
        {
            for (var idx = 0; idx < _colors.Length; idx++)
            {
                if (!_colors[idx].HasValue) return idx;
            }

            return -1;
        }

        protected void SetColorAt(int idx, TvColor color)
        {
            _colors[idx] = color;
        }
        
        public TvColor this[string name] => _colorIndexes.TryGetValue(name, out var index)  ? (_colors[index] ?? TvColor.Black) : TvColor.Black;

        public TvColor this[int idx] => _colors[idx] ?? TvColor.Black;

        public IEnumerable<(int, TvColor)> Entries
        {
            get
            {
                for (var idx = 0; idx < _colors.Length; idx++)
                {
                    var color = _colors[idx];
                    if (color.HasValue)
                    {
                        yield return (idx, color.Value);
                    }
                }
            }
        }
        
        protected void LoadPalette(string terminalName, IPaletteDefinitionParser parser, UpdateTerminalEntries whenInvokeCallback)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var stream =
                assembly.GetManifestResourceStream(
                    $"Tvision2.ConsoleDriver.ColorDefinitions.{terminalName}.txt");
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                var idx = 0;
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var parsedLine = parser.ParseLine(line);
                    if (parsedLine.idx < MaxColors)
                    {
                        AddColor(parsedLine.idx, parsedLine.rgbColor, parsedLine.name);
                        var invoke = (whenInvokeCallback == UpdateTerminalEntries.All) ||
                                     (whenInvokeCallback == UpdateTerminalEntries.AllButAnsi3bit &&
                                      parsedLine.idx > TvColor.ANSI3BIT_MAX_VALUE) ||
                                     (whenInvokeCallback == UpdateTerminalEntries.AllButAnsi4bit &&
                                      parsedLine.idx > TvColor.ANSI4BIT_MAX_VALUE);
                        if (invoke)
                        {
                            OnColorAdded(parsedLine.rgbColor, parsedLine.idx);
                        }
                    }
                }
            }
        
        }
        
        protected virtual void OnColorAdded(TvColor color, int idx)
        {
        }

        
    }
}