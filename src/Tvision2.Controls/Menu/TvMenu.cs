using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Controls.List;
using Tvision2.Controls.Styles;
using Tvision2.Core.Engine;
using Tvision2.Core.Render;

namespace Tvision2.Controls.Menu
{
    public class TvMenu : TvControl<MenuState>
    {
        private readonly TvMenuBarOptions _options;
        private TvList<MenuEntry> _list;
        private IComponentTree _ownerUi;
        private readonly ISkin _skin;

        public TvMenu(ITvControlCreationParametersBuilder<MenuState> parameters, Action<ITvMenuBarOptions> optionsAction = null) : this(parameters.Build(), optionsAction) { }
        public TvMenu(TvControlCreationParameters<MenuState> parameters, Action<ITvMenuBarOptions> optionsAction = null) : base(parameters)
        {
            _options = new TvMenuBarOptions();
            optionsAction?.Invoke(_options);
            Metadata.CanFocus = false;
            _skin = parameters.Skin;
        }

        public static ITvControlCreationParametersBuilder<MenuState> CreationParametersBuilder(IEnumerable<string> options)
        {
            return TvControlCreationParametersBuilder.ForState<MenuState>(() => new MenuState(options));
        }

        protected override void ConfigureMetadataOptions(TvControlMetadataOptions options)
        {
            options.AvoidDrawControl();
        }

        protected override void OnViewportCreated(IViewport viewport)
        {
            var builder = TvList.CreationParametersBuilder(State.Entries);
            builder.UseSkin(_skin);
            builder.UseTopLeftPosition(viewport.Position);
            _list = new TvList<MenuEntry>(builder);
        }

        protected override void OnControlMounted(ITuiEngine engine)
        {
            _ownerUi = engine.UI;
            _ownerUi.Add(_list);
        }

    }
}
