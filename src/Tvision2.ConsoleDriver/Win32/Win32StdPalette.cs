using System.Linq;
using Tvision2.Core;
using Tvision2.Core.Colors;

namespace Tvision2.ConsoleDriver.Win32
{
    internal class Win32StdPalette : IPalette
    {
        private readonly string[] _names;
        public bool IsFreezed => true;

        public int MaxColors => TvColorNames.StandardColorsCount;

        public ColorMode ColorMode => ColorMode.Basic;

        public bool RedefineColor(int idx, TvColor newColor) => false;
        
        public Win32StdPalette()
        {
            _names = TvColorNames.AlLStandardColorNames.ToArray();
        }

        
        public TvColor this[string name]
        {
            get
            {
                for (var idx = 0; idx < _names.Length; idx++)
                {
                    if (_names[idx] == name)
                    {
                        return TvColor.FromRaw(idx);
                    }
                }
                
                return TvColor.Black;
            }
        }
        
        public TvColor this[int idx]
        {
            get
            {
                if (idx < TvColorNames.StandardColorsCount)
                {
                    return TvColor.FromRaw(idx);
                }

                return TvColor.Black;
            }
        }

    }
}