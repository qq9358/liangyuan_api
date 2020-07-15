using System;
using System.Threading.Tasks;

namespace Egoal.BackgroundJobs
{
    public interface IBackgroundJobService
    {
        Task<string> EnqueueAsync<TJob>(string args, BackgroundJobPriority priority = BackgroundJobPriority.Normal, TimeSpan? delay = null) where TJob : IBackgroundJob;

        Task<bool> DeleteAsync(long jobId);
    }
}
