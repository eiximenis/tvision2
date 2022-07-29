using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tvision2.Core.Components;

namespace Tvision2.Layouts
{
    public interface ICellContainer
    {
        public void Add(TvComponent child);
        public ICellContainer WithAlignment(ChildAlignment alignment);
        public TvComponent? Get();
    }
}
