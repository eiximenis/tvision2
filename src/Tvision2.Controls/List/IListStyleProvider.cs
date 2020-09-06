using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Colors;
using Tvision2.Styles;

namespace Tvision2.Controls.List
{
    public interface IListStyleProvider<T>
    {
        IListStyleProviderConditionBuilder<T> Use (TvColor fore, TvColor back);
        IListStyleProviderConditionBuilder<T> Use(TvColor fore);
        IListStyleProvider<T> UseSkin(ISkin skin);


    }

    public interface IListStyleProviderConditionBuilder<T>
    {
        IListStyleProviderColumnSelector<T> When(Func<T, bool> predicate);
    }

    public interface IListStyleProviderColumnSelector<T>
    {
        IListStyleProvider<T> AppliesToColumn(int idx);
        IListStyleProvider<T> AppliesToAllColumns();
    }
}
