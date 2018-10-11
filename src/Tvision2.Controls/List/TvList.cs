using System.Collections.Generic;
using Tvision2.Controls.Drawers;
using Tvision2.Controls.Styles;
using Tvision2.Core.Components;
using Tvision2.Core.Components.Behaviors;
using Tvision2.Core.Render;

namespace Tvision2.Controls.List
{
    public class TvList<TItem> : TvControl<ListState<TItem>>
    {
        public TvList(ISkin skin, IViewport boxModel, ListState<TItem> data) : base(skin, boxModel, data)
        {
        }

        protected override void AddCustomElements(TvComponent<ListState<TItem>> component)
        {
            component.AddDrawer(new BorderDrawer(CurrentStyle, Metadata));
        }

        protected override IEnumerable<ITvBehavior<ListState<TItem>>> GetEventedBehaviors()
        {
            yield return new ListBehavior<TItem>();
        }

        protected override void OnDraw(RenderContext<ListState<TItem>> context)
        {
            var pairIdx = CurrentStyle.Standard;
            var selectedPairIdx = Metadata.IsFocused ? CurrentStyle.AlternateFocused : CurrentStyle.Alternate;
            var viewport = context.Viewport;
            var numitems = State.Count;
            State.ItemsView.Adjust(viewport.Rows - 2);
            for (var idx = 0; idx < viewport.Rows - 2; idx++)
            {
                if (idx < State.ItemsView.NumItems)
                {
                    var item = State.ItemsView[idx];
                    var selected = State.SelectedIndex == idx + State.ItemsView.From;
                    var len = item.Text.Length;
                    context.DrawStringAt(item.Text, new TvPoint(1, idx + 1), selected ? selectedPairIdx : pairIdx);
                    var extra = viewport.Columns - 2 - len;
                    if (extra > 0)
                    {
                        context.DrawChars(' ', extra, new TvPoint(len + 1, idx + 1), pairIdx);
                    }
                }
                else
                {
                    context.DrawChars(' ', viewport.Columns - 2, new TvPoint(1, idx + 1), pairIdx);
                }
            }
        }
    }
}
