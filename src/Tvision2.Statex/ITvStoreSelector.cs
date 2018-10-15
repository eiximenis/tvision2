using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Statex
{
    public interface ITvStoreSelector
    {
        ITvStore GetStore(string name);

        ITvStore<TState> GetStore<TState>(string name) where TState : class;
        ITvStore<TState> GetStore<TState>() where TState : class;
    }
}
