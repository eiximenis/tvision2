using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Controls.List;
using Tvision2.Core.Engine;
using Tvision2.Core.Render;

namespace Tvision2.Controls.Menu
{
    public class TvMenu : TvControl<MenuState>
    {
        private readonly TvMenuBarOptions _options;
        private TvList<MenuEntry> _list;
        private IComponentTree _ownerUi;

        public TvMenu(ITvControlCreationParametersBuilder<MenuState> parameters, Action<ITvMenuBarOptions> optionsAction = null) : this(parameters.Build(), optionsAction) { }
        public TvMenu(TvControlCreationParameters<MenuState> parameters, Action<ITvMenuBarOptions> optionsAction = null) : base(parameters)
        {
            _options = new TvMenuBarOptions();
            optionsAction?.Invoke(_options);
            var builder = TvList.CreationParametersBuilder(State.Options);
            builder.UseSkin(parameters.Skin);
            builder.UseViewport(CalculateViewport());
            _list = new TvList<MenuEntry>(builder);
            Metadata.CanFocus = false;
        }

        private IViewport CalculateViewport()
        {
            throw new NotImplementedException();
        }

        public static ITvControlCreationParametersBuilder<MenuState> CreationParametersBuilder(IEnumerable<string> options)
        {
            return TvControlCreationParametersBuilder.ForState<MenuState>(() => new MenuState(options));
        }

        protected override void ConfigureMetadataOptions(TvControlMetadataOptions options)
        {
            options.AvoidDrawControl();
        }

        protected override void OnControlMounted(IComponentTree owner)
        {
            _ownerUi = owner;
            _ownerUi.Add(_list);
        }

    }
}
