using System;
using Tvision2.Core.Hooks;
using Tvision2.Events;

namespace Tvision2.Controls.Menu
{
    internal class TvMenuBarHook : IEventHook
    {
        private readonly ConsoleKey _hotkey;
        private readonly TvControlMetadata _metadata;

        public TvMenuBarHook(ConsoleKey hotkey, TvControlMetadata metadata)
        {
            _hotkey = hotkey;
            _metadata = metadata;
        }

        public void ProcessEvents(TvConsoleEvents events, HookContext context)
        {
            var @event = events.AcquireFirstKeyboard(evt => evt.IsKeyDown && evt.AsConsoleKeyInfo().Key == _hotkey);
            if (@event != null)
            {
                _metadata.Focus(force: true);
            }
        }
    }
}