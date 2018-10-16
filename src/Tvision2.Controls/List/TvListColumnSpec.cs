using System;

namespace Tvision2.Controls.List
{
    public class TvListColumnSpec<T>
    {
        public int Width { get; set; }
        public Func<T, string> Transformer { get; set; }
    }
}