using Tvision2.Core.Stores;

namespace Tvision2.Core.Engine
{
    public interface ITvDispatcher
    {
        void Dispatch(TvAction action);
    }
}