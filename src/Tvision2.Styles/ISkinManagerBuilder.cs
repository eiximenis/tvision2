using System;

namespace Tvision2.Styles
{
    public interface ISkinManagerBuilder
    {
        ISkinManagerBuilder AddDefaultSkin(Action<ISkinBuilder> builderOptions);
        ISkinManagerBuilder AddSkin(string name, Action<ISkinBuilder> builderOptions);
    }
}