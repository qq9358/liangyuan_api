using Egoal.Domain.Repositories;
using System.Threading.Tasks;

namespace Egoal.BackgroundJobs
{
    public interface IBackgroundJobRepository : IRepository<BackgroundJobInfo, long>
    {
        Task<BackgroundJobInfo> GetWaitingJobAsync();
        Task<bool> LockJobAsync(BackgroundJobInfo jobInfo);
        Task<bool> UnLockJobAsync(BackgroundJobInfo jobInfo);
        Task<bool> UpdateJobAsync(BackgroundJobInfo jobInfo);
        Task<bool> DeleteJobAsync(BackgroundJobInfo jobInfo);
    }
}
