using System;
using System.Threading;
using System.Threading.Tasks;

namespace Tvision2.Core.Engine
{
    public interface ITuiEngine
    {
        Task Start(CancellationToken cancellationToken);
        IComponentTree UI { get; }
        IEventHookManager EventHookManager { get; }
        IServiceProvider ServiceProvider { get; }
    }
}