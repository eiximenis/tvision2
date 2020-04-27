using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Controls.List;
using Tvision2.Controls.Styles;
using Tvision2.Core.Engine;
using Tvision2.Core.Render;
using Tvision2.Controls.Extensions;

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

            var builder = TvList.CreationParametersBuilder<MenuEntry>(
                    () => ListState<MenuEntry>
                        .From(State.Entries)
                        .AddColumn(me => me.Text)
                        .Build());
            builder.UseSkin(_skin);
            builder.UseViewport(viewport);
            //builder.UseTopLeftPosition(viewport.Position);
            _list = new TvList<MenuEntry>(builder);
            _list.AsComponent()
               .Metadata.OnComponentMounted
               .AddOnce(ctx =>
               {
                   _list.Metadata.Focus(force: true);
               });
        }


        protected override void OnControlMounted(ITuiEngine engine)
        {
            _ownerUi = engine.UI;
            _ownerUi.AddAsChild(_list, this);
            Metadata.CaptureFocus();
        }

        protected override void OnControlUnmounted(ITuiEngine engine)
        {
            _ownerUi.Remove(_list);
            _ownerUi = null;
        }
    }
}
