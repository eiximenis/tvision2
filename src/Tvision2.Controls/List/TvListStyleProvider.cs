using System;
using System.Collections.Generic;
using System.Linq;
using Tvision2.Controls.Styles;
using Tvision2.Core.Colors;

namespace Tvision2.Controls.List
{
    public class TvListStyleProvider<T> : IListStyleProvider<T>, IListStyleProviderConditionBuilder<T>, IListStyleProviderColumnSelector<T>
    {
        private const int ALL_COLUMNS = -1;
        class ListStyleProviderItem
        {
            public Func<T, bool> Predicate { get; set; }
            public TvisionColor Fore { get; set; }
            public TvisionColor Back { get; set; }
            public int ColumnIdx { get; set; } = ALL_COLUMNS;
        }

        private readonly IColorManager _colorManager;
        private List<ListStyleProviderItem> _items;
        private IStyle _style;

        public TvListStyleProvider(IColorManager colorManager)
        {
            _colorManager = colorManager;
            _items = new List<ListStyleProviderItem>();
        }

        public IListStyleProvider<T> UseSkin(ISkin skinToUse)
        {
            _style = skinToUse["tvlist"];
            return this;
        }

        public IListStyleProviderConditionBuilder<T> Use(TvisionColor fore, TvisionColor back)
        {
            _items.Add(new ListStyleProviderItem()
            {
                Back = back,
                Fore = fore
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

        public CharacterAttribute GetAttributesForItem(T item, int colidx)
        {
            foreach (var comparison in _items.Where(i => i.ColumnIdx == ALL_COLUMNS || i.ColumnIdx == colidx))
            {
                if (comparison.Predicate(item)) return _colorManager.BuildAttributeFor(comparison.Fore, comparison.Back, CharacterAttributeModifiers.Normal);
            }

            return _style?.Standard ?? _colorManager.DefaultAttribute;
        }
    }

}
