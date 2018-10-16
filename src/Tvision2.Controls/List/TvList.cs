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
        protected readonly TvListStyleProvider<TItem> _styleProvider;

        public ICommand<TItem> OnItemClicked { get; set; }

        public IListStyleProvider<TItem> StyleProvider => _styleProvider;

        public TvList(ISkin skin, IViewport boxModel, ListState<TItem> data) : base(skin, boxModel, data)
        {
            _styleProvider = new TvListStyleProvider<TItem>(skin.ColorManager);
            _styleProvider.UseSkin(skin);
        }

        protected override void AddCustomElements(TvComponent<ListState<TItem>> component)
        {
            component.AddDrawer(new BorderDrawer(CurrentStyle, Metadata));
        }

        protected override IEnumerable<ITvBehavior<ListState<TItem>>> GetEventedBehaviors()
        {
            yield return new ListBehavior<TItem>(async () => await (OnItemClicked?.Invoke(State[State.SelectedIndex]) ?? Task.CompletedTask));
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
                    var tvitem = CreateTvItem(item);
                    for (var column = 0; column < State.Columns.Length; column++)
                    {
                        var coldef = State.Columns[column];
                        var colWidth = coldef.Width > 0 ? coldef.Width : viewport.Columns - 2 - State.ColumnsTotalFixedWidth;
                        var text = tvitem.Text[column];
                        var attr = tvitem.Attribute[column];
                        context.DrawStringAt(text.PadRight(colWidth), new TvPoint(1 + lenDrawn, idx + 1), selected ? selectedAttr : attr);
                        lenDrawn += colWidth;
                    }
                }
                else
                {
                    var attr = CurrentStyle.Standard;
                    context.DrawChars(' ', viewport.Columns - 2, new TvPoint(1, idx + 1), attr);
                }
            }
        }

        private TvListItem CreateTvItem(TItem item)
        {
            var numColumns = State.Columns.Length;
            var tvitem = new TvListItem();
            tvitem.Text = State.Columns.Select(c => c.Transformer(item)).ToArray();
            tvitem.Attribute = Enumerable.Range(0, numColumns)
                .Select(idx => _styleProvider.GetAttributesForItem(item, idx))
                .ToArray();

            return tvitem;
        }
    }
}
