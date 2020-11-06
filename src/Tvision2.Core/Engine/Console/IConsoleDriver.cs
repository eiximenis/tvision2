using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Colors;
using Tvision2.Core.Render;
using Tvision2.Events;

namespace Tvision2.Engine.Console
{
    public interface IConsoleDriver
    {
        IColorManager ColorManager { get; }
        void WriteCharacterAt(int x, int y, char character, CharacterAttribute attribute);
        void WriteCharactersAt(int x, int y, int count,  char character, CharacterAttribute attribute);
        void SetCursorAt(int x, int y);
        ITvConsoleEvents ReadEvents();
        void Init();
        TvBounds ConsoleBounds { get; } 
        void SetCursorVisibility(bool isVisible);
        void ProcessWindowEvent(TvWindowEvent windowEvent);

        void End() { }
    }
}
