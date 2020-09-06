using System;

namespace Tvision2.Styles
{
    public interface ISkinManagerBuilder
    {
        ISkinManagerBuilder AddSkin(string name, Action<ISkinBuilder> builderOptions);
    }
}