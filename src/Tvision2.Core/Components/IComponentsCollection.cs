using System.Collections.Generic;

namespace Tvision2.Core.Components
{
    public interface IComponentsCollection : IEnumerable<TvComponent>
    {
        void Clear();
        void Add(TvComponent child);
        int Count { get; }
        bool Remove(TvComponent component);


    }
}
