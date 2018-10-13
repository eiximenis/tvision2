using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Controls.Styles;
using Tvision2.Core.Colors;

namespace Tvision2.Controls.List
{
    public class TvListStyleProvider<T> : IListStyleProvider<T>, IListStyleProviderConditionBuilder<T>
    {

        class ListStyleProviderItem
        {
            public Func<T, bool> Predicate { get; set; }
            public DefaultColorName Fore { get; set; }
            public DefaultColorName Back { get; set; }
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

        public IListStyleProviderConditionBuilder<T> Use(DefaultColorName fore, DefaultColorName back)
        {
            _items.Add(new ListStyleProviderItem()
            {
                Back = back,
                Fore = fore
            });
            return this;
        }

        public IListStyleProvider<T> When(Func<T, bool> predicate)
        {

            _items[_items.Count - 1].Predicate = predicate;
            return this;
        }

        public CharacterAttribute GetAttributesForItem(T item)
        {
            foreach (var comparison in _items)
            {
                if (comparison.Predicate(item)) return _colorManager.BuildAttributeFor(comparison.Fore, comparison.Back, CharacterAttributeModifiers.Normal);
            }

            return  _style?.Standard ?? _colorManager.DefaultAttribute;
        }
    }

}
