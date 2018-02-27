using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Components;

namespace Tvision2.Core.Engine
{
    public interface IComponentTree
    {
        IEnumerable<TvComponent> Responders { get; }
        void ClearResponders();
        void AddToResponderChain(TvComponent componentToAdd);
        void AddToResponderChain(TvComponentMetadata componentToAdd);
        void RemoveFromRespondersChain(TvComponentMetadata componentToRemove);
        void RemoveFromRespondersChain(TvComponent componentToRemove);
    }
}
