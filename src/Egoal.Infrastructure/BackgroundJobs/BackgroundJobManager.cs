using Egoal.Extensions;
using Egoal.Logging;
using Egoal.Threading.BackgroundWorkers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Egoal.BackgroundJobs
{
    public class BackgroundJobManager : PeriodicBackgroundWorkerBase
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger _logger;

        public BackgroundJobManager(
            IServiceProvider serviceProvider,
            ILogger<BackgroundJobManager> logger)
            : base(logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;

            Period = TimeSpan.FromSeconds(5);
        }

        protected override async Task DoWorkAsync(CancellationToken stoppingToken)
        {
            try
            {
                var waitingJob = await GetAndLockWaitingJobAsync();
                if (waitingJob == null)
                {
                    Period = TimeSpan.FromSeconds(5);

                    return;
                }

                if (stoppingToken.IsCancellationRequested)
                {
                    await UnLockJobAsync(waitingJob);

                    return;
                }

                await TryProcessJob(waitingJob, stoppingToken);

                Period = TimeSpan.FromMilliseconds(1);
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
            }
        }

        private async Task TryProcessJob(BackgroundJobInfo jobInfo, CancellationToken stoppingToken)
        {
            var scope = _serviceProvider.CreateScope();

            try
            {
                jobInfo.TryCount++;
                jobInfo.LastTryTime = DateTime.Now;

                var jobType = Type.GetType(jobInfo.JobType);
                var job = scope.ServiceProvider.GetService(jobType) as IBackgroundJob;

                try
                {
                    await job.ExecuteAsync(jobInfo.JobArgs, stoppingToken);

                    await DeleteJobAsync(jobInfo);
                }
                catch (Exception ex)
                {
                    _logger.LogException(ex);

                    if (ex.IsWebException())
                    {
                        jobInfo.TryCount--;
                        jobInfo.NextTryTime = DateTime.Now.AddSeconds(15);
                    }
                    else
                    {
                        var nextTryTime = jobInfo.CalculateNextTryTime();
                        if (nextTryTime.HasValue)
                        {
                            jobInfo.NextTryTime = nextTryTime.Value;
                        }
                        else
                        {
                            jobInfo.IsAbandoned = true;
                        }
                    }

                    await UpdateJobAsync(jobInfo);
                }
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);

                jobInfo.IsAbandoned = true;

                await UpdateJobAsync(jobInfo);
            }
            finally
            {
                scope.Dispose();
            }
        }

        private async Task<BackgroundJobInfo> GetAndLockWaitingJobAsync()
        {
            return await UseStoreAsync(async store =>
            {
                return await store.GetAndLockWaitingJobAsync();
            });
        }

        private async Task<bool> UnLockJobAsync(BackgroundJobInfo jobInfo)
        {
            return await UseStoreAsync(async store =>
            {
                return await store.UnLockJobAsync(jobInfo);
            });
        }

        private async Task<bool> UpdateJobAsync(BackgroundJobInfo jobInfo)
        {
            return await UseStoreAsync(async store =>
            {
                return await store.UpdateJobAsync(jobInfo);
            });
        }

        private async Task<bool> DeleteJobAsync(BackgroundJobInfo jobInfo)
        {
            return await UseStoreAsync(async store =>
            {
                return await store.DeleteJobAsync(jobInfo);
            });
        }

        private async Task<T> UseStoreAsync<T>(Func<IBackgroundJobStore, Task<T>> func)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var store = scope.ServiceProvider.GetRequiredService<IBackgroundJobStore>();

                return await func(store);
            }
        }
    }
}
