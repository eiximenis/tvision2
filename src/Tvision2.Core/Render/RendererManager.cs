using Tvision2.Engine.Console;

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
