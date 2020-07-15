using Dapper;
using Egoal.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Egoal.BackgroundJobs
{
    public class BackgroundJobRepository : EfCoreRepositoryBase<BackgroundJobInfo, long>, IBackgroundJobRepository
    {
        public BackgroundJobRepository(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {

        }

        public async Task<BackgroundJobInfo> GetWaitingJobAsync()
        {
            var now = DateTime.Now;

            string sql = @"
SELECT TOP 1
* 
FROM dbo.BackgroundJob WITH(UPDLOCK)
WHERE IsAbandoned=0
AND NextTryTime<=@now
AND (IsLocked=0 OR (IsLocked=1 AND LockEndTime<=@now))
ORDER BY [Priority] DESC,TryCount,NextTryTime
";
            return await Connection.QueryFirstOrDefaultAsync<BackgroundJobInfo>(sql, new { now }, Transaction);
        }

        public async Task<bool> LockJobAsync(BackgroundJobInfo jobInfo)
        {
            string sql = @"
UPDATE dbo.BackgroundJob SET
IsLocked=1,
LockEndTime=@LockEndTime
WHERE Id=@Id
";
            return (await Connection.ExecuteAsync(sql, new { jobInfo.Id, jobInfo.LockEndTime }, Transaction)) > 0;
        }

        public async Task<bool> UnLockJobAsync(BackgroundJobInfo jobInfo)
        {
            string sql = @"
UPDATE dbo.BackgroundJob SET
IsLocked=0,
LockEndTime=NULL
WHERE Id=@Id
";
            return (await Connection.ExecuteAsync(sql, new { jobInfo.Id }, Transaction)) > 0;
        }

        public async Task<bool> UpdateJobAsync(BackgroundJobInfo jobInfo)
        {
            string sql = @"
UPDATE dbo.BackgroundJob SET
TryCount=@TryCount,
LastTryTime=@LastTryTime,
NextTryTime=@NextTryTime,
IsAbandoned=@IsAbandoned,
IsLocked=0,
LockEndTime=NULL
WHERE Id=@Id
";
            return (await Connection.ExecuteAsync(sql, new { jobInfo.Id, jobInfo.TryCount, jobInfo.LastTryTime, jobInfo.NextTryTime, jobInfo.IsAbandoned }, Transaction)) > 0;
        }

        public async Task<bool> DeleteJobAsync(BackgroundJobInfo jobInfo)
        {
            string sql = "DELETE dbo.BackgroundJob WHERE Id=@Id";

            return (await Connection.ExecuteAsync(sql, new { jobInfo.Id }, Transaction)) > 0;
        }
    }
}
