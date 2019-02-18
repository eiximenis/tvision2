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

        public TvMenuBar(ITvControlCreationParametersBuilder<MenuBarState> parameters, Action<ITvMenuBarOptions> optionsAction = null) : this(parameters.Build()) { }
        public TvMenuBar(TvControlCreationParameters<MenuBarState> parameters, Action<ITvMenuBarOptions> optionsAction = null) : base(parameters)
        {
            _options = new TvMenuBarOptions();
            optionsAction?.Invoke(_options);
        }
        public static ITvControlCreationParametersBuilder<MenuBarState> CreationParametersBuilder(IEnumerable<string> options)
        {
            return TvControlCreationParametersBuilder.ForState<MenuBarState>(() => new MenuBarState(options));
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
