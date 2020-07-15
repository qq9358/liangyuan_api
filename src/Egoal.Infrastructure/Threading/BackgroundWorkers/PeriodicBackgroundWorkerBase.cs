using Egoal.Logging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Egoal.Threading.BackgroundWorkers
{
    public abstract class PeriodicBackgroundWorkerBase : BackgroundService
    {
        private readonly ILogger _logger;

        public TimeSpan Period { get; set; }

        public PeriodicBackgroundWorkerBase(ILogger logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            string typeName = GetType().Name;

            _logger.LogDebug($"{typeName} is starting.");

            stoppingToken.Register(() => _logger.LogDebug($"{typeName} is stopping."));

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await DoWorkAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogException(ex);
                }

                await Task.Delay(Period, stoppingToken);
            }

            _logger.LogDebug($"{typeName} is stopping.");
        }

        protected abstract Task DoWorkAsync(CancellationToken stoppingToken);
    }
}
