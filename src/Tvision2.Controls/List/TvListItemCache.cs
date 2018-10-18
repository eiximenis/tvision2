using System.Collections.Generic;
using System.Linq;

namespace Tvision2.Controls.List
{
    class TvListItemCache<TItem>
    {
        private readonly TvListColumnSpec<TItem>[] _columns;
        private readonly TvListStyleProvider<TItem> _styleProvider;
        private readonly Dictionary<int, TvListItem> _cachedItems;

        public TvListItemCache(TvListColumnSpec<TItem>[] columns, TvListStyleProvider<TItem> styleProvider)
        {
            _cachedItems = new Dictionary<int, TvListItem>();
            _columns = columns;
            _styleProvider = styleProvider;
        }

        public TvListItem GetTvItemFor(TItem item, int idx, int variableWidth)
        {
            if (!_cachedItems.TryGetValue(idx, out TvListItem tvitem))
            {
                var numColumns = _columns.Length;
                tvitem = new TvListItem();
                tvitem.Texts = _columns.Select(c => c.Transformer(item).PadRight(c.Width > 0 ? c.Width : variableWidth)).ToArray();
                tvitem.Attributes = Enumerable.Range(0, numColumns)
                    .Select(colidx => _styleProvider.GetAttributesForItem(item, colidx))
                    .ToArray();

                _cachedItems.Add(idx, tvitem);
            }
            return tvitem;
        }

    }
}
