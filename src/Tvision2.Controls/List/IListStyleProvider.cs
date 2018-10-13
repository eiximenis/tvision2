using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Controls.Styles;
using Tvision2.Core.Colors;

namespace Tvision2.Controls.List
{
    public interface IListStyleProvider<T>
    {
        IListStyleProviderConditionBuilder<T> Use (DefaultColorName fore, DefaultColorName back);
        IListStyleProvider<T> UseSkin(ISkin skin);
    }

    public interface IListStyleProviderConditionBuilder<T>
    {
        IListStyleProvider<T> When(Func<T, bool> predicate);
    }
}
