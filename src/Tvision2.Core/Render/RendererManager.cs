using System;
using System.Collections.Generic;
using System.Text;

using Console = TvConsole.TvConsole;

namespace Tvision2.Core.Render
{
    public static class RendererManager
    {
        public static void DrawAt(ConsoleCharacter @char, int left, int top)
        {
            Console.Instance.WriteCharacterAt(left, top, @char.Character, @char.Foreground, @char.Background);
        }
    }
}
