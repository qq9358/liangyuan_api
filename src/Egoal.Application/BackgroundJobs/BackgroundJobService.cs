using Egoal.Application.Services;
using Egoal.Domain.Uow;
using System;
using System.Threading.Tasks;
using System.Transactions;

namespace Egoal.BackgroundJobs
{
    public class BackgroundJobService : ApplicationService, IBackgroundJobService, IBackgroundJobStore
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IBackgroundJobRepository _backgroundJobRepository;

        public BackgroundJobService(
            IUnitOfWorkManager unitOfWorkManager,
            IBackgroundJobRepository backgroundJobRepository)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _backgroundJobRepository = backgroundJobRepository;
        }

        public async Task<string> EnqueueAsync<TJob>(string args, BackgroundJobPriority priority = BackgroundJobPriority.Normal, TimeSpan? delay = null)
            where TJob : IBackgroundJob
        {
            var jobInfo = new BackgroundJobInfo
            {
                JobType = typeof(TJob).AssemblyQualifiedName,
                JobArgs = args,
                Priority = priority
            };

            if (delay.HasValue)
            {
                jobInfo.NextTryTime = DateTime.Now.Add(delay.Value);
            }

            var id = await _backgroundJobRepository.InsertAndGetIdAsync(jobInfo);

            return id.ToString();
        }

        public async Task<bool> DeleteAsync(long jobId)
        {
            await _backgroundJobRepository.DeleteAsync(o => o.Id == jobId);
            return true;
        }

        public async Task<BackgroundJobInfo> GetAndLockWaitingJobAsync()
        {
            using (var uow = _unitOfWorkManager.Begin(TransactionScopeOption.RequiresNew))
            {
                var waitingJob = await _backgroundJobRepository.GetWaitingJobAsync();

                if (waitingJob != null)
                {
                    waitingJob.LockEndTime = DateTime.Now.AddMinutes(30);
                    await _backgroundJobRepository.LockJobAsync(waitingJob);
                }

                await uow.CompleteAsync();

                return waitingJob;
            }
        }

        public async Task<bool> UnLockJobAsync(BackgroundJobInfo jobInfo)
        {
            using (var uow = _unitOfWorkManager.Begin(TransactionScopeOption.RequiresNew))
            {
                var result = await _backgroundJobRepository.UnLockJobAsync(jobInfo);

                await uow.CompleteAsync();

                return result;
            }
        }

        public async Task<bool> UpdateJobAsync(BackgroundJobInfo jobInfo)
        {
            using (var uow = _unitOfWorkManager.Begin(TransactionScopeOption.RequiresNew))
            {
                var result = await _backgroundJobRepository.UpdateJobAsync(jobInfo);

                await uow.CompleteAsync();

                return result;
            }
        }

        public async Task<bool> DeleteJobAsync(BackgroundJobInfo jobInfo)
        {
            using (var uow = _unitOfWorkManager.Begin(TransactionScopeOption.RequiresNew))
            {
                var result = await _backgroundJobRepository.DeleteJobAsync(jobInfo);

                await uow.CompleteAsync();

                return result;
            }
        }
    }
}
