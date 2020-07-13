using BlazorTerm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Principal;
using System.Text;
using Tvision2.Core.Colors;
using Tvision2.Core.Render;
using Tvision2.Engine.Console;
using Tvision2.Events;

namespace Tvision2.ConsoleDriver.BlazorTerm
{
    public class BtermConsoleDriver : IConsoleDriver
    {

        private BTermComponent _terminal;

        private readonly BlazorTermColorManager _colorManager;

        public IColorManager ColorManager => _colorManager;

        public TvBounds ConsoleBounds => new TvBounds().Grow(24, 80);

        public BtermConsoleDriver()
        {
            _colorManager = new BlazorTermColorManager();
        }

        public void BoundToBterm(BTermComponent terminal)
        {
            _terminal = terminal;
        }

        public void Init()
        {
            
        }

        public void ProcessWindowEvent(TvWindowEvent windowEvent)
        {
        }

        public ITvConsoleEvents ReadEvents()
        {
            return TvConsoleEvents.Empty;
        }

        public void SetCursorAt(int x, int y)
        {
            
        }

        public void SetCursorVisibility(bool isVisible)
        {
            
        }

        public void WriteCharacterAt(int x, int y, char character, CharacterAttribute attribute)
        {
            Debug.WriteLine($"WriteCharacterAt {character} @ ({x},{y})");
            _terminal?.WriteAt(x, y, character);
        }

        public void WriteCharactersAt(int x, int y, int count, char character, CharacterAttribute attribute)
        {
            Debug.WriteLine($"WriteCharactersAt {character} x {count} @ ({x},{y})");
            if (_terminal != null)
            {
                for (int rep = 0; rep < count; rep++)
                {
                    _terminal.WriteAt(x + rep, y, character);
                }
            }
        }
    }
}
