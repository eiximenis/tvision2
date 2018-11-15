using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tvision2.Controls.Drawers;
using Tvision2.Controls.Styles;
using Tvision2.Core.Components;
using Tvision2.Core.Components.Behaviors;
using Tvision2.Core.Render;

namespace Tvision2.Controls.List
{
    public class TvList<TItem> : TvControl<ListState<TItem>>
    {

        private readonly TvListItemCache<TItem> _itemsCache;
        protected readonly TvListStyleProvider<TItem> _styleProvider;
        private readonly TvListOptions<TItem> _options;

        public ICommandChain<TItem> OnItemClicked { get; }

        public IListStyleProvider<TItem> StyleProvider => _styleProvider;

        public TvList(ISkin skin, IViewport boxModel, ListState<TItem> data, Action<ITvListOptions<TItem>> optionsAction = null) : base(skin, boxModel, data)
        {
            _options = new TvListOptions<TItem>();
            optionsAction?.Invoke(_options);
            OnItemClicked = new CommandChain<TItem>();
            _styleProvider = new TvListStyleProvider<TItem>(skin.ColorManager);
            _styleProvider.UseSkin(skin);
            _itemsCache = new TvListItemCache<TItem>(State.Columns, _styleProvider);
            State.SetCache(_itemsCache);
        }

        protected override void AddCustomElements(TvComponent<ListState<TItem>> component)
        {
            component.AddDrawer(new BorderDrawer(CurrentStyle, Metadata));
        }

        protected override IEnumerable<ITvBehavior<ListState<TItem>>> GetEventedBehaviors()
        {
            yield return new ListBehavior<TItem>(async () => await (OnItemClicked.Invoke(State[State.SelectedIndex]) ?? Task.CompletedTask));
        }

        protected override void OnDraw(RenderContext<ListState<TItem>> context)
        {
            var selectedAttr = Metadata.IsFocused ? CurrentStyle.AlternateFocused : CurrentStyle.Alternate;
            var viewport = context.Viewport;
            var numitems = State.Count;
            State.ItemsView.Adjust(viewport.Rows - 2);
            for (var idx = 0; idx < viewport.Rows - 2; idx++)
            {
                if (idx < State.ItemsView.NumItems)
                {
                    var lenDrawn = 0;
                    var item = State.ItemsView[idx];
                    var selected = State.SelectedIndex == idx + State.ItemsView.From;
                    var tvitem = _itemsCache.GetTvItemFor(item, idx + State.ItemsView.From, viewport.Columns - 2 - State.ColumnsTotalFixedWidth);
                    for (var column = 0; column < State.Columns.Length; column++)
                    {
                        var text = tvitem.Texts[column];
                        var attr = tvitem.Attributes[column];
                        context.DrawStringAt(text, new TvPoint(1 + lenDrawn, idx + 1), selected ? selectedAttr : attr);
                        lenDrawn += text.Length;
                    }
                }
                else
                {
                    var attr = CurrentStyle.Standard;
                    context.DrawChars(' ', viewport.Columns - 2, new TvPoint(1, idx + 1), attr);
                }
            }
        }

    }
}
