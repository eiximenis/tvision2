using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Engine;

namespace Tvision2.Core
{
    public interface ICustomItemsProviderClient
    {
        void ReceiveProvider(ICustomItemsProvider provider);
    }
}
