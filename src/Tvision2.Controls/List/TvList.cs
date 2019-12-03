﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tvision2.Controls.Drawers;
using Tvision2.Core;
using Tvision2.Core.Components;
using Tvision2.Core.Components.Behaviors;
using Tvision2.Core.Render;
using Tvision2.Controls.Extensions;

namespace Tvision2.Controls.List
{
    public static class TvList
    {
        public static ITvControlCreationParametersBuilder<ListState<TItem>> CreationParametersBuilder<TItem>(Func<ListState<TItem>> stateCreator)
        {
            return TvControlCreationParametersBuilder.ForState(stateCreator);
        }

        public static ITvControlCreationParametersBuilder<ListState<TItem>> CreationParametersBuilder<TItem>(IEnumerable<TItem> initialData)
        {
            return TvControlCreationParametersBuilder.ForState(() => ListState.FromEnumerable(initialData));
        }


    }

    public class TvList<TItem> : TvControl<ListState<TItem>>
    {

        private readonly TvListItemCache<TItem> _itemsCache;
        protected readonly TvListStyleProvider<TItem> _styleProvider;
        private readonly TvListOptions<TItem> _options;

        private readonly ActionChain<TItem> _onItemClicked;

        public IActionChain<TItem> OnItemClicked => _onItemClicked;

        public IListStyleProvider<TItem> StyleProvider => _styleProvider;

        public TvList(ITvControlCreationParametersBuilder<ListState<TItem>> parameters, Action<ITvListOptions<TItem>> optionsAction = null) : this(parameters.Build()) { }
        public TvList(TvControlCreationParameters<ListState<TItem>> parameters, Action<ITvListOptions<TItem>> optionsAction = null) : base(parameters)
        {
            _options = new TvListOptions<TItem>();
            optionsAction?.Invoke(_options);
            _onItemClicked = new ActionChain<TItem>();
            _styleProvider = new TvListStyleProvider<TItem>();
            _styleProvider.UseSkin(parameters.Skin);
            _itemsCache = new TvListItemCache<TItem>(State.Columns, _styleProvider);
            State.SetCache(_itemsCache);
        }

        protected override void AddCustomElements(TvComponent<ListState<TItem>> component)
        {
            component.AddDrawer(new BorderDrawer(CurrentStyle, Metadata));
        }

        protected override IEnumerable<ITvBehavior<ListState<TItem>>> GetEventedBehaviors()
        {
            yield return new ListBehavior<TItem>(() => _onItemClicked.Invoke(State[State.SelectedIndex]));
        }

        protected override void OnDraw(RenderContext<ListState<TItem>> context)
        {
            var selectedAttr = Metadata.IsFocused ? CurrentStyle.AlternateFocused : CurrentStyle.Alternate;
            var viewport = context.Viewport;
            var numitems = State.Count;
            State.ItemsView.Adjust(viewport.Bounds.Rows - 2);
            for (var idx = 0; idx < viewport.Bounds.Rows - 2; idx++)
            {
                if (idx < State.ItemsView.NumItems)
                {
                    var lenDrawn = 0;
                    var item = State.ItemsView[idx];
                    var selected = State.SelectedIndex == idx + State.ItemsView.From;
                    var tvitem = _itemsCache.GetTvItemFor(item, idx + State.ItemsView.From, viewport.Bounds.Cols - 2 - State.ColumnsTotalFixedWidth);
                    for (var column = 0; column < State.Columns.Length; column++)
                    {
                        var text = tvitem.Texts[column];
                        var attr = tvitem.Attributes[column];
                        if (selected)
                        {
                            context.DrawStringAt(text, TvPoint.FromXY(1 + lenDrawn, idx + 1), selectedAttr);
                        }
                        else
                        {
                            if (attr.Background == null)
                            {
                                attr = new Styles.StyleEntry(attr.Foreground, CurrentStyle.Standard.Background, attr.Attributes);
                            }
                            context.DrawStringAt(text, TvPoint.FromXY(1 + lenDrawn, idx + 1), attr);
                        }

                        lenDrawn += text.Length;
                    }
                    var remaining = viewport.Bounds.Cols - 2 - lenDrawn;
                    if (remaining > 0)
                    {
                        context.DrawChars(' ', remaining, TvPoint.FromXY(1 + lenDrawn, idx + 1), selected ? selectedAttr : CurrentStyle.Standard);
                    }
                }
                else
                {
                    var attr = CurrentStyle.Standard;
                    context.DrawChars(' ', viewport.Bounds.Cols - 2, TvPoint.FromXY(1, idx + 1), attr);
                }
            }
        }

    }
}
