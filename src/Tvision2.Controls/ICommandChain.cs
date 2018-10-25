using System;
using System.Linq;
using System.Threading.Tasks;

namespace Tvision2.Controls
{
    public interface ICommandChain
    {
        Guid Add(ICommand command);
        void Remove(Guid id);
    }
    public interface ICommandChain<TData> : ICommandChain
    {
        Guid Add(ICommand<TData> command);
        Task Invoke(TData item);
    }
}
