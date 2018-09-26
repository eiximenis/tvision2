using System;

namespace Tvision2.Controls.Styles
{
    public interface ISkinBuilder
    {

        ISkinBuilder AddBaseStyle (Action<IStyleBuilder> builderOptions);
        ISkinBuilder AddStyle(string name, Action<IStyleBuilder> builderOptions);

    }
}