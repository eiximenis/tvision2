using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tvision2.Layouts.Stack
{
    public class StackLayout
    {
        private readonly List<LayoutSize> _sizes;
        public int ItemsCount => _sizes.Count;

        public int TotalFixed => _sizes.Sum(s => s.Fixed);
        public int TotalVariable => _sizes.Sum(s => s.Variable);

        public IEnumerable<LayoutSize> Items => _sizes;

        public LayoutSize this[int idx] => _sizes[idx];

        public StackLayout()
        {
            _sizes = new List<LayoutSize>();
        }


        public void Add (params string[] reps)
        {
            _sizes.AddRange(reps.Select(r => LayoutSize.FromString(r)));
        }

        public void Add(LayoutSize itemSize)
        {
            _sizes.Add(itemSize);
        }

        public int GetRealSize(LayoutSize size, int totalSize)
        {

            if (size.Fixed > 0) return size.Fixed;

            var totalFixed = TotalFixed;
            var totalVariable = TotalVariable;

            if (totalVariable == 0) return 0;

            var remainingVariable = totalSize - totalFixed;
            var asteriskValue = remainingVariable / totalVariable;

            return size.Variable * asteriskValue;

        }
    }
}
