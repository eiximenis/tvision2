using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Core.Engine
{
    public interface ICustomItemsProvider
    {
        TItem GetCustomItem<TItem>();
    }
}
