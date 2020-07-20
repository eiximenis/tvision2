
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Security.Principal;
using System.Text;
using Tvision2.ConsoleDriver.Ansi;
using Tvision2.ConsoleDriver.Common;
using Tvision2.Core.Colors;
using Tvision2.Core.Render;
using Tvision2.Engine.Console;
using Tvision2.Events;

namespace Tvision2.ConsoleDriver.Blazor
{

    public class BtermConsoleDriver : IConsoleDriver
    {
        private readonly AnsiColorManager _colorManager;
        private readonly BtermEventReceiver _eventReceiver;
        private IJSRuntime _jsRuntime;
        private bool _bound;

        public IColorManager ColorManager => _colorManager;

        public TvBounds ConsoleBounds => new TvBounds().Grow(24, 80);

        public BtermConsoleDriver(AnsiColorManager colorManager)
        {
            _colorManager = colorManager;
            _eventReceiver = new BtermEventReceiver();
            _bound = false;
        }


        public void Init() { }

        public void ProcessWindowEvent(TvWindowEvent windowEvent)
        {
        }

        public ITvConsoleEvents ReadEvents()
        {
            var events = _eventReceiver.CurrentEvents;
            _eventReceiver.DeleteAllEventsOnNextCycle();
            return events;
        }

        public void SetCursorAt(int x, int y)
        {
            var seq = XtermSequences.CursorAt(x, y);
            ((IJSInProcessRuntime)_jsRuntime).InvokeVoid("tv$.write", seq);
        }

        public void BindToTerminal(string id, IJSRuntime jsRuntime)
        {

            _jsRuntime = jsRuntime;
            var jsEventReceiverRef = DotNetObjectReference.Create(_eventReceiver);
            ((IJSInProcessRuntime)_jsRuntime).InvokeVoid("tv$.bindTerminal", id, jsEventReceiverRef);
            _bound = true;

        }

        public void SetCursorVisibility(bool isVisible)
        {

        }

        public void WriteCharacterAt(int x, int y, char character, CharacterAttribute attribute)
        {
            var seq = XtermSequences.CursorAt(x, y);
            seq = seq + _colorManager.GetAttributeSequence(attribute);
            seq = seq + character.ToString();
            ((IJSInProcessRuntime)_jsRuntime).InvokeVoid("tv$.write", seq);

        }

        public void WriteCharactersAt(int x, int y, int count, char character, CharacterAttribute attribute)
        {
            for (int rep = 0; rep < count; rep++)
            {
                WriteCharacterAt(x + rep, y, character, attribute);
            }
        }

        static class XtermSequences
        {
            internal static string CursorAt(int x, int y)
            {
                return string.Format("\x1b[{0};{1}H", y, x);
            }
        }
    }
}
