using System;
using System.Collections.Generic;
using System.Linq;
using Tvision2.Controls.Styles;
using Tvision2.Core.Colors;
using Tvision2.Core.Render;

namespace Tvision2.Controls.List
{
    public class TvListStyleProvider<T> : IListStyleProvider<T>, IListStyleProviderConditionBuilder<T>, IListStyleProviderColumnSelector<T>
    {
        private const int ALL_COLUMNS = -1;
        class ListStyleProviderItem
        {
            public Func<T, bool> Predicate { get; set; }
            public TvColor Foreground { get; set; }
            public TvColor Background { get; set; }
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

        public IListStyleProviderConditionBuilder<T> Use(TvColor fore, TvColor back)
        {
            _items.Add(new ListStyleProviderItem()
            {
                Background = back,
                Foreground = fore
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

        public TvColorPair GetColorsForItem(T item, int colidx)
        {
            foreach (var comparison in _items.Where(i => i.ColumnIdx == ALL_COLUMNS || i.ColumnIdx == colidx))
            {
                if (comparison.Predicate(item)) return new TvColorPair(comparison.Foreground, comparison.Background);
            }

            // TODO: Search for a way to invoke tje real "GetColorFor" of the Background.
            // Right now we assume row 0, but this can look ugly if list Background is not
            // fixed color
            return new TvColorPair(_style.Standard.Foreground, _style.Standard.Background.GetColorFor(0,colidx, TvBounds.Empty));

        }
    }

}
