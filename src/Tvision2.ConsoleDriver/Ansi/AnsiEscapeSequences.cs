namespace Tvision2.ConsoleDriver.Ansi
{
    public static class AnsiEscapeSequences
    {

        public const string SMCUP = "\x1b[?1049h";                    // Enter TUI mode
        public const string CLEAR = "\x1b[2J";                        // Clear screen
        
        public const string DECTCEM_VISIBLE = "\x1b[25h";             // Show cursor
        public const string DECTCEM_HIDDEN = "\x1b[25l";              // Hide  cursor

        public const string INITC = "\x1b[4;{0};rgb:{1:x}{2:x}/{3:x}\x1b\\";     // Change palette color at index
        
    }
}