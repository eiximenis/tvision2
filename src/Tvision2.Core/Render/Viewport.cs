namespace Tvision2.Core.Render
{
    public sealed class Viewport : IViewport
    {
        private static Viewport _nullViewport = new Viewport(new TvPoint(0, 0), 0, 0, 0);
        public TvPoint Position { get; }

        public int ZIndex { get;  }

        public int Columns { get; }

        public int Rows { get; }

        public static IViewport NullViewport => _nullViewport;

        public Viewport(TvPoint point, int cols) : this (point, cols, 1, 0)
        {
        }

        public Viewport(TvPoint point, int cols, int rows, int zindex)
        {
            Position = point;
            Columns = cols;
            Rows = rows;
            ZIndex = zindex;
        }



        public IViewport ResizeTo(int cols, int rows) => new Viewport(Position, cols, rows, ZIndex);
        public IViewport Grow(int ncols, int nrows) => new Viewport(Position, Columns + ncols, Rows + nrows, ZIndex);

        public IViewport MoveTo(TvPoint newPos) => new Viewport(newPos, Columns, Rows, ZIndex);

        public IViewport Translate(TvPoint translation) => new Viewport(Position + translation, Columns, Rows, ZIndex);

        public IViewport Clone() => new Viewport(Position, Columns, Rows, ZIndex);
        


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
                && Columns == other.Columns && other.Rows == Rows
                && ZIndex == other.ZIndex;
        }
    }
}
