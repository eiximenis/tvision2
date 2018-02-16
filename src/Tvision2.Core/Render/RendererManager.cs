using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.ConsoleDriver;

namespace Tvision2.Core.Render
{
    public static class RendererManager
    {
        public static void DrawAt(IConsoleDriver console, ConsoleCharacter @char, int left, int top)
        {
            console.WriteCharacterAt(left, top, @char.Character, @char.Foreground, @char.Background);
        }
    }
}
