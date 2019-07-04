using System.Linq.Expressions;

namespace Tvision2.Core.Colors
{
    public interface IPalette
    {
        bool IsFreezed { get; }
        int MaxColors { get; }

        bool RedefineColor(int idx, TvColor newColor);

        ColorMode ColorMode { get; }
        
        TvColor this[string name] { get; }
        TvColor this[int idx] { get; }

    }
}