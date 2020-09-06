using System;

namespace Tvision2.Styles
{
    public interface ISkinBuilder
    {

        ISkinBuilder AddBaseStyle (Action<IStyleBuilder> builderOptions);
        ISkinBuilder AddStyle(string name, Action<IStyleBuilder> builderOptions);

    }
}