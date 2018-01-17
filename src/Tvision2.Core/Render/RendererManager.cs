using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Core.Render
{
    public static class RendererManager
    {
        public static void DrawAt(ConsoleCharacter @char, int left, int top)
        {
            Console.SetCursorPosition(left, top);
            Console.BackgroundColor = @char.Background;
            Console.ForegroundColor = @char.Foreground;
            Console.Write(@char.Character);
        }
    }
}
