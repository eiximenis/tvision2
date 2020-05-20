using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Controls.Styles;
using Tvision2.Core.Components.Behaviors;
using Tvision2.Core.Engine;
using Tvision2.Core.Render;
using Tvision2.Controls.Extensions;
using Tvision2.Core.Components;
using System.Linq;

namespace Tvision2.Controls.Menu
{
    public class TvMenuBar : TvControl<MenuState>
    {

        private readonly TvMenuBarOptions _options;
        private Guid _hookGuid;
        protected readonly TvControlCreationParameters _creationParameters;
        private ITuiEngine _engine;
        private TvMenu _currentMenu;

        public TvMenuBar(ITvControlCreationParametersBuilder<MenuState> parameters, Action<ITvMenuBarOptions> optionsAction = null) : this(parameters.Build(), optionsAction) { }


        public TvMenuBar(TvControlCreationParameters<MenuState> parameters, Action<ITvMenuBarOptions> optionsAction = null) : base(parameters)
        {
            _creationParameters = parameters;
            _hookGuid = Guid.Empty;
            _options = new TvMenuBarOptions();
            optionsAction?.Invoke(_options);
            _engine = null;
            _currentMenu = null;
        }
        public static ITvControlCreationParametersBuilder<MenuState> CreationParametersBuilder(IEnumerable<string> options)
        {
            return TvControlCreationParametersBuilder.ForState<MenuState>(() => new MenuState(options));
        }

        protected override void ConfigureMetadataOptions(TvControlMetadataOptions options)
        {
            options.IsFocused().OnlyWhenAnyChildHasFocus();
        }

        protected override void OnDraw(RenderContext<MenuState> context)
        {
            var coordx = 0;
            var optidx = 0;

            var selectedItemPairIdx = Metadata.IsFocused ? CurrentStyle.Focused : CurrentStyle.Standard;
            var selectedItemAlternatePairidx = Metadata.IsFocused ? CurrentStyle.AlternateFocused : CurrentStyle.Alternate;

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
            yield return new MenuBarBehavior(_options);
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

            _currentMenu = new TvMenu(TvMenu.CreationParametersBuilder(entry.ChildEntries.Select(e => e.Text))
                .UseViewport(new Viewport(TvPoint.FromXY(10, 1), TvBounds.FromRowsAndCols(10, 20), Layer.Top))
                .UseSkin(_creationParameters.Skin));
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
