using System.Threading.Tasks;

namespace Egoal.BackgroundJobs
{
    public interface IBackgroundJobStore
    {
        Task<BackgroundJobInfo> GetAndLockWaitingJobAsync();
        Task<bool> UnLockJobAsync(BackgroundJobInfo jobInfo);
        Task<bool> UpdateJobAsync(BackgroundJobInfo jobInfo);
        Task<bool> DeleteJobAsync(BackgroundJobInfo jobInfo);
    }
}
