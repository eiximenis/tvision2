namespace Tvision2.Core.Render
{
    public class VirtualConsoleCursor
    {
        public bool MovementPending { get; internal set; }
        public TvPoint Position { get; private set; }

        public VirtualConsoleCursor()
        {
            Position = new TvPoint(0, 0);
        }

        public void MoveTo(int left, int top)
        {
            Position = new TvPoint(left, top);
            MovementPending = true;
        }
    }
}