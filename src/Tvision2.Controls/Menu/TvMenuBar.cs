using System;
using System.Collections.Generic;
using Tvision2.Core.Components.Behaviors;
using Tvision2.Core.Engine;
using Tvision2.Core.Render;
using Tvision2.Styles.Extensions;
using System.Linq;

namespace Tvision2.Controls.Menu
{

    public class TvMenuBarParamsBuilder : TvControlCreationBuilder<TvMenuBar, MenuState, ITvMenuBarOptionsBuilder, ITvMenuBarOptions, TvMenuBarOptions> { }

    public class TvMenuBar : TvControl<MenuState, ITvMenuBarOptions>
    {

        private readonly ITvMenuBarOptions _options;
        private Guid _hookGuid;
        protected readonly TvControlCreationParameters _creationParameters;
        private ITuiEngine _engine;
        private TvMenu _currentMenu;

        public static ITvControlOptionsBuilder<TvMenuBar, MenuState, ITvMenuBarOptionsBuilder, ITvMenuBarOptions, TvMenuBarOptions> UseParams() => new TvMenuBarParamsBuilder();


        public TvMenuBar(TvControlCreationParameters<MenuState, ITvMenuBarOptions> parameters) : base(parameters)
        {
            _creationParameters = parameters;
            _hookGuid = Guid.Empty;
            _options = parameters.Options;
            _engine = null;
            _currentMenu = null;
        }


        protected override void ConfigureMetadataOptions(TvControlMetadataOptions options)
        {
            options.FocusedWhen().OnlyAnyChildHasFocus();
        }

        protected override void OnDraw(RenderContext<MenuState> context)
        {
            var coordx = 0;
            var optidx = 0;

            var selectedItemPairIdx = Metadata.IsFocused ? CurrentStyle.Active : CurrentStyle.Standard;
            var selectedItemAlternatePairidx = Metadata.IsFocused ? CurrentStyle.AlternateActive : CurrentStyle.Alternate;

            foreach (var option in State.Entries)
            {
                var pairIdx = State.SelectedIndex == optidx ? selectedItemPairIdx : CurrentStyle.Standard;
                var alternatePairIdx = State.SelectedIndex == optidx ? selectedItemAlternatePairidx : CurrentStyle.Alternate;
                var text = option.Text;
                for (var pos = 0; pos < text.Length; pos++)
                {
                    var pairIdxToUse = pos == option.ShortcutPos ? alternatePairIdx : pairIdx;
                    context.DrawStringAt($"{option.Text[pos]}", TvPoint.FromXY(coordx + pos, 0), pairIdxToUse);
                }

                coordx += option.Text.Length;
                context.DrawChars(' ', _options.SpaceBetweenItems, TvPoint.FromXY(coordx, 0), pairIdx);
                coordx += _options.SpaceBetweenItems;
                optidx++;
            }

            context.DrawChars(' ', context.Viewport.Bounds.Cols - coordx, TvPoint.FromXY(coordx, 0), CurrentStyle.Standard);
        }

        protected override IEnumerable<ITvBehavior<MenuState>> GetEventedBehaviors()
        {
            yield return new MenuBarBehavior(Options);
        }

        protected override void OnControlMounted(ITuiEngine engine)
        {
            _engine = engine;
            if (_options.Hotkey != ConsoleKey.NoName)
            {
                var evtHook = engine.EventHookManager;
                _hookGuid = evtHook.AddHook(new TvMenuBarHook(_options.Hotkey, Metadata));
            }
        }

        protected override void OnControlUnmounted(ITuiEngine engine)
        {
            _engine = null;
            if (_hookGuid != Guid.Empty)
            {
                var evtHook = engine.EventHookManager;
                evtHook.RemoveHook(_hookGuid);
            }
        }

        internal void EnableMenu(MenuState state)
        {
            var entry = state.SelectedEntry;
            if (!entry.ChildEntries.Any()) return;


            var menuParams = TvMenu.UseParams().WithState(new MenuState(entry.ChildEntries.Select(e => e.Text)))
                .Configure(c => c.UseViewport(new Viewport(TvPoint.FromXY(10, 1), TvBounds.FromRowsAndCols(10, 20), Layer.Top))
                .UseSkin(_creationParameters.Skin))
                .Build();
            _currentMenu = new TvMenu(menuParams);
            _engine.UI.AddAsChild(_currentMenu, this);
        }

        internal void UpdateCurrentMenu()
        {
            if (_currentMenu == null) return;
            _engine.UI.Remove(_currentMenu);
            Metadata.Focus(force: true);
            EnableMenu(State);
        }



        internal void CloseCurrentMenu()
        {
            if (_currentMenu != null)
            {
                _engine.UI.Remove(_currentMenu);
                Metadata.Focus(force: true);
                _currentMenu = null;
            }
            else
            {
                Metadata.ReturnFocusToPrevious();
            }
        }


    }
}
