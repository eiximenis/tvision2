using System;

namespace Tvision2.Controls.Styles
{
    public interface ISkinBuilder
    {
        ISkinBuilder UseDefaultStyles(IStyleSheet styles);
        ISkinBuilder AddStyleSheet(string name, IStyleSheet sheet);
        ISkinBuilder AddStyleSheet(string name, Action<IStyleSheetBuilder> builderOptions);
    }
}