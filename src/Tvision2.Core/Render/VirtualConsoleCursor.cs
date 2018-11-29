namespace Tvision2.Core.Render
{

    public enum VirtualConsoleCursorAction
    {
        None,
        Hide,
        Show
    }
    public class VirtualConsoleCursor
    {
        public VirtualConsoleCursorAction ActionPending { get; private set; }

        public bool MovementPending { get; internal set; }
        public TvPoint Position { get; private set; }

        public bool Visible { get; private set; }

        internal void VisibilityChanged(bool isVisible)
        {
            Visible = isVisible;
            ActionPending = VirtualConsoleCursorAction.None;
        }

        public VirtualConsoleCursor()
        {
            ActionPending = VirtualConsoleCursorAction.None;
            Position = new TvPoint(0, 0);
        }

        public void MoveTo(int left, int top)
        {
            if (!Visible)
            {
                ActionPending = VirtualConsoleCursorAction.Show;
            }

            if (Position.Left != left || Position.Top != top)
            {
                Position = new TvPoint(left, top);
                MovementPending = true;
            }
        }

        public void Hide()
        {
            ActionPending = VirtualConsoleCursorAction.Hide;
        }
    }
}