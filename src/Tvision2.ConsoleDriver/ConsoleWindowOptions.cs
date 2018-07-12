namespace Tvision2.ConsoleDriver
{
    public class ConsoleWindowOptions : IConsoleWindowOptions
    {
        
        public int Rows { get; private set; }

        public int Cols { get; private set; }

        public ConsoleWindowOptions()
        {
            Rows = Cols = -1;
        }

        IConsoleWindowOptions IConsoleWindowOptions.Size(int rows, int cols)
        {
            Rows = rows;
            Cols = cols;
            return this;
        }
    }
}