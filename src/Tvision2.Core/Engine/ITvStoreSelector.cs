using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Stores;

namespace Tvision2.Core.Engine
{
    public interface ITvStoreSelector
    {
        ITvStore<TState> GetStore<TState>(string name) where TState : class;
    }
}
