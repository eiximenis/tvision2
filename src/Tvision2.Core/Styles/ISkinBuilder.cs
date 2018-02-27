using System;

namespace Tvision2.Core.Styles
{
    public interface ISkinBuilder
    {
        ISkinBuilder UseDefaultStyle(IBaseStyles styles);
        ISkinBuilder AddStyle(string name, IBaseStyles style);
        ISkinBuilder AddStyle(string name, Action<IBaseStylesBuilder> builderOptions);
    }
}