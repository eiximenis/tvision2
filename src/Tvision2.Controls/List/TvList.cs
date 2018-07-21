using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tvision2.Controls.Drawers;
using Tvision2.Controls.Styles;
using Tvision2.Core.Components;
using Tvision2.Core.Components.Behaviors;
using Tvision2.Core.Render;

namespace Tvision2.Controls.List
{
    public class TvList :  TvControl<ListState>
    {
        public TvList(ISkin skin, IViewport boxModel, ListState data) : base(skin, boxModel, data)
        {
        }

        protected override void AddCustomElements(TvComponent<ListState> component)
        {
            component.AddDrawer(new BorderDrawer(Style));
        }

        protected override IEnumerable<ITvBehavior<ListState>> GetEventedBehaviors()
        {
            yield return new ListBehavior();
        }

        protected override void OnDraw(RenderContext<ListState> context)
        {
            var viewport = context.Viewport;
            var numitems = State.Count;
            State.ItemsView.Adjust(viewport.Rows - 2);
            for (var idx= 0; idx < viewport.Rows - 2; idx++)
            {
                if (idx < State.ItemsView.NumItems)
                {
                    var item = State.ItemsView[idx];
                    var selected = State.SelectedIndex == idx + State.ItemsView.From;
                    var len = item.Length;
                    context.DrawStringAt(item, new TvPoint(1, idx + 1), selected ? Style.HiliteForeColor : Style.ForeColor, selected ? Style.HiliteBackColor : Style.BackColor);
                    var extra = viewport.Columns - 2 - len;
                    if (extra > 0)
                    {
                        context.DrawChars(' ', extra, new TvPoint(len + 1, idx + 1), Style.ForeColor, Style.BackColor);
                    }
                }
                else
                {
                    context.DrawChars(' ', viewport.Columns - 2, new TvPoint(1, idx + 1), Style.ForeColor, Style.BackColor);
                }
            }
        }
    }
}
