using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Controls.Styles;
using Tvision2.Core.Engine;
using Tvision2.Core.Render;

namespace Tvision2.Controls.Menu
{
    public class TvMenuBar : TvControl<MenuBarState>
    {

        private readonly TvMenuBarOptions _options;
        private Guid _hookGuid;

        public TvMenuBar(ITvControlCreationParametersBuilder<MenuBarState> parameters, Action<ITvMenuBarOptions> optionsAction = null) : this(parameters.Build(), optionsAction) { }
        public TvMenuBar(TvControlCreationParameters<MenuBarState> parameters, Action<ITvMenuBarOptions> optionsAction = null) : base(parameters)
        {
            _hookGuid = Guid.Empty;
            _options = new TvMenuBarOptions();
            optionsAction?.Invoke(_options);
            Metadata.CanFocus = false;
        }
        public static ITvControlCreationParametersBuilder<MenuBarState> CreationParametersBuilder(IEnumerable<string> options)
        {
            return TvControlCreationParametersBuilder.ForState<MenuBarState>(() => new MenuBarState(options));
        }

        protected override void OnDraw(RenderContext<MenuBarState> context)
        {
            var coordx = 0;
            var optidx = 0;

            var selectedItemPairIdx = Metadata.IsFocused ? CurrentStyle.Focused : CurrentStyle.Standard;
            var selectedItemAlternatePairidx = Metadata.IsFocused ? CurrentStyle.AlternateFocused : CurrentStyle.Alternate;

            foreach (var option in State.Options)
            {
                var pairIdx = State.SelectedIndex == optidx ? selectedItemPairIdx : CurrentStyle.Standard;
                var alternatePairIdx = State.SelectedIndex == optidx ? selectedItemAlternatePairidx : CurrentStyle.Alternate;
                var text = option.Text;
                for (var pos = 0; pos < text.Length; pos++)
                {
                    var pairIdxToUse = pos == option.ShortcutPos ? alternatePairIdx : pairIdx;
                    context.DrawStringAt($"{option.Text[pos]}", new TvPoint(coordx + pos, 0), pairIdxToUse);
                }

                coordx += option.Text.Length;
                context.DrawChars(' ', _options.SpaceBetweenItems, new TvPoint(coordx, 0), pairIdx);
                coordx += _options.SpaceBetweenItems;
                optidx++;
            }

            context.DrawChars(' ', Viewport.Columns - coordx, new TvPoint(coordx, 0), CurrentStyle.Standard);
        }

        protected override void OnControlMounted(IComponentTree owner)
        { 
            if (_options.Hotkey != ConsoleKey.NoName)
            {
                var evtHook = AsComponent().Metadata.Engine.EventHookManager;
                _hookGuid =  evtHook.AddHook(new TvMenuBarHook(_options.Hotkey, Metadata));
            }
        }

        protected override void OnControlUnmounted(IComponentTree owner)
        {
            if (_hookGuid != Guid.Empty)
            {
                var evtHook = AsComponent().Metadata.Engine.EventHookManager;
                evtHook.RemoveHook(_hookGuid);
            }
        }
    }
}
