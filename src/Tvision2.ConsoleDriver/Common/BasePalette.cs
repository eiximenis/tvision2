using System.Collections.Generic;
using Tvision2.Core;
using Tvision2.Core.Colors;

namespace Tvision2.ConsoleDriver.Common
{
    abstract  class BasePalette
    {
        private TvColor[] _colors;
        private readonly Dictionary<string, int> _colorIndexes;

        public int MaxColors { get; protected set; }


        public BasePalette()
        {
            _colorIndexes = new Dictionary<string, int>();
        }

        protected void InitSize(int size)
        {
            _colors = new TvColor[size];
        }

        protected void SetColorAt(int idx, TvColor color, string name = null)
        {
            _colors[idx] = color;
            if (name != null)
            {
                _colorIndexes.Add(name, idx);
            }
        }
        
        
        public TvColor this[string name]
        {
            get
            {
                if (_colorIndexes.TryGetValue(name, out var index))
                {
                    return _colors[index];
                }

                return TvColor.Black;        // TODO: Think what we can do here...
            }
        }

        public TvColor this[int idx]
        {
            get { return _colors[idx]; }
        }
    }
}