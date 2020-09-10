using System;
using System.Collections.Generic;
using Tvision2.Controls.List;
using Tvision2.Core.Engine;
using Tvision2.Core.Render;
using Tvision2.Core.Components.Behaviors;
using Tvision2.Styles;

namespace Tvision2.Controls.Menu
{
    public class TvMenuParamsBuilder : TvControlCreationBuilder<TvMenu, MenuState> { }
    public class TvMenu : TvControl<MenuState>
    {
        private readonly TvMenuBarOptions _options;
        private TvList<MenuEntry> _list;
        private readonly ISkin _skin;

        public TvMenu(TvControlCreationParameters<MenuState> parameters, Action<ITvMenuBarOptions> optionsAction = null) : base(parameters)
        {
            _options = new TvMenuBarOptions();
            optionsAction?.Invoke(_options);
            _skin = parameters.Skin;
        }



        public static ITvControlOptionsBuilder<TvMenu, MenuState> UseParams() => new TvMenuParamsBuilder();

        protected override IEnumerable<ITvBehavior<MenuState>> GetEventedBehaviors()
        {
            yield return new MenuBehavior(this);
        }


        protected override void ConfigureMetadataOptions(TvControlMetadataOptions options)
        {
            options.AvoidDrawControl();
            options.IsFocused().Never();
        }

        protected override void OnViewportCreated(IViewport viewport)
        {

            var listParams = TvList.UseParams<MenuEntry>()
                .WithState(() => ListState<MenuEntry>
                        .From(State.Entries)
                        .AddColumn(me => me.Text)
                        .Build())
                .Configure(c => c.UseSkin(_skin).UseViewport(viewport))
                .Build();
            //builder.UseTopLeftPosition(viewport.Position);
            _list = new TvList<MenuEntry>(listParams);
            _list.AsComponent()
               .Metadata.OnComponentMounted
               .AddOnce(ctx =>
               {
                   _list.Metadata.Focus(force: true);
               });
        }


        protected override void OnControlMounted(ITuiEngine engine)
        { 
            engine.UI.AddAsChild(_list, this);
            Metadata.CaptureFocus();
        }
    }
}
