namespace Tvision2.Core.Render
{
    public sealed class Viewport : IViewport
    {
        private static Viewport _nullViewport = new Viewport(TvPoint.Zero, TvBounds.Empty, 0);
        public TvPoint Position { get; }
        public TvBounds Bounds { get; }

        public int ZIndex { get; }


        public static IViewport NullViewport => _nullViewport;

        public Viewport(TvPoint point, int cols) : this(point, new TvBounds(0, cols), 0)
        {
        }

        public Viewport(TvPoint point, TvBounds bounds, int zindex)
        {
            Position = point;
            Bounds = bounds;
            ZIndex = zindex;
        }




        public IViewport ResizeTo(int cols, int rows) => new Viewport(Position, new TvBounds(rows, cols), ZIndex);
        public IViewport Grow(int ncols, int nrows) => new Viewport(Position, Bounds.Grow(nrows, ncols), ZIndex);

        public IViewport MoveTo(TvPoint newPos) => new Viewport(newPos, Bounds, ZIndex);

        public IViewport Translate(TvPoint translation) => new Viewport(Position + translation, Bounds, ZIndex);

        public IViewport Clone() => new Viewport(Position, Bounds, ZIndex);



        public override bool Equals(object obj)
        {
            if (obj is IViewport)
            {
                return Equals((IViewport)obj);
            }
            else
            {
                return base.Equals(obj);
            }
        }

        public bool Equals(IViewport other)
        {
            return Position == other.Position
                && Bounds == other.Bounds
                && ZIndex == other.ZIndex;
        }
    }
}
