using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Tvision2.Core.Engine
{
    internal class TuiEngineHost : IHostedService
    {
        private readonly ITuiEngine _engine;
        private Task _uiTask;
        private readonly CancellationTokenSource _cancelTokenSource;
        public TuiEngineHost(ITuiEngine engine)
        {
            _engine = engine ?? throw new ArgumentNullException(nameof(engine));
            _cancelTokenSource = new CancellationTokenSource(); 
        }

        Task IHostedService.StartAsync(CancellationToken cancellationToken)
        {
            _uiTask = _engine.Start(_cancelTokenSource.Token);
            return _uiTask.IsCompleted ? _uiTask : Task.CompletedTask;

        }
        async Task IHostedService.StopAsync(CancellationToken cancellationToken)
        {
            if (_uiTask == null)
            {
                return;
            }

            try
            {
                _cancelTokenSource.Cancel();
            }
            finally
            {
                await Task.WhenAny(_uiTask, Task.Delay(Timeout.Infinite, cancellationToken));
            }
        }

    }
}
