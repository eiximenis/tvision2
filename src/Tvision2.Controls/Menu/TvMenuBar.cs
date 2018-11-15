using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Controls.Styles;
using Tvision2.Core.Render;

namespace Tvision2.Controls.Menu
{
    public class TvMenuBar : TvControl<MenuBarState>
    {

        private readonly TvMenuBarOptions _options;

        public TvMenuBar(ISkin skin, IViewport boxModel, MenuBarState data, Action<ITvMenuBarOptions> optionsAction = null) : base(skin, boxModel, data)
        {
            _options = new TvMenuBarOptions();
            optionsAction?.Invoke(_options);
        }

        protected override void OnDraw(RenderContext<MenuBarState> context)
        {
            var pairIdx = Metadata.IsFocused ? CurrentStyle.Focused : CurrentStyle.Standard;
            var coordx = 0;
            foreach (var option in State.Options)
            {
                context.DrawStringAt($"{option}", new TvPoint(coordx, 0), pairIdx);
                coordx += option.Length;
                context.DrawChars(' ', _options.SpaceBetweenItems, new TvPoint(coordx, 0), pairIdx);
                coordx += _options.SpaceBetweenItems;
            }

            context.DrawChars(' ', Viewport.Columns - coordx, new TvPoint(coordx, 0), pairIdx);
        }
    }
}
