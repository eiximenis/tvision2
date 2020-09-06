using System;
using System.Collections.Generic;
using System.Linq;
using Tvision2.Core.Colors;
using Tvision2.Styles;
using Tvision2.Styles.Backgrounds;

namespace Tvision2.Controls.List
{
    public class TvListStyleProvider<T> : IListStyleProvider<T>, IListStyleProviderConditionBuilder<T>, IListStyleProviderColumnSelector<T>
    {
        private const int ALL_COLUMNS = -1;
        class ListStyleProviderItem
        {
            public Func<T, bool> Predicate { get; set; }
            public StyleEntry Style { get; set; }
            public int ColumnIdx { get; set; } = ALL_COLUMNS;
        }

        private readonly IColorManager _colorManager;
        private List<ListStyleProviderItem> _items;
        private IStyle _style;

        public TvListStyleProvider()
        {
            _items = new List<ListStyleProviderItem>();
        }

        public IListStyleProvider<T> UseSkin(ISkin skinToUse)
        {
            _style = skinToUse["tvlist"];
            return this;
        }

        public IListStyleProviderConditionBuilder<T> Use(TvColor fore)
        {
            _items.Add(new ListStyleProviderItem()
            {
                Style = new StyleEntry(fore, null, CharacterAttributeModifiers.Normal)
            });

            return this;
        }

        public IListStyleProviderConditionBuilder<T> Use(TvColor fore, TvColor back)
        {
            _items.Add(new ListStyleProviderItem()
            {
                Style = new StyleEntry(fore, new SolidColorBackgroundProvider(back),CharacterAttributeModifiers.Normal)
            });
            return this;
        }

        public IListStyleProviderColumnSelector<T> When(Func<T, bool> predicate)
        {

            _items[_items.Count - 1].Predicate = predicate;
            return this;
        }

        public IListStyleProvider<T> AppliesToColumn(int idx)
        {
            _items[_items.Count - 1].ColumnIdx = idx;
            return this;
        }

        public IListStyleProvider<T> AppliesToAllColumns()
        {
            _items[_items.Count - 1].ColumnIdx = ALL_COLUMNS;
            return this;
        }

        public StyleEntry GetStyleForItem(T item, int colidx)
        {
            foreach (var comparison in _items.Where(i => i.ColumnIdx == ALL_COLUMNS || i.ColumnIdx == colidx))
            {
                if (comparison.Predicate(item)) return comparison.Style;
            }

            return _style.Standard;
        }
    }

}
